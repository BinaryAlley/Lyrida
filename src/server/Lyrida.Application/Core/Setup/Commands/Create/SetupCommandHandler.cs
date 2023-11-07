#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using ErrorOr;
using System.Linq;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Common.Errors;
using Lyrida.DataAccess.Repositories.Users;
using Lyrida.DataAccess.Repositories.Setup;
using Lyrida.DataAccess.Repositories.Roles;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.DataAccess.Repositories.UserRoles;
using Lyrida.Infrastructure.Core.Authentication;
using Lyrida.DataAccess.Repositories.Permissions;
using Lyrida.Application.Common.DTO.Authorization;
using Lyrida.Application.Common.DTO.Authentication;
using Lyrida.DataAccess.Repositories.Configuration;
using Lyrida.Application.Common.DTO.Configuration;
#endregion

namespace Lyrida.Application.Core.Setup.Commands.Create;

/// <summary>
/// Application setup command handler
/// </summary>
/// <remarks>
/// Creation Date: 15th of August, 2023
/// </remarks>
public class SetupCommandHandler : IRequestHandler<SetupCommand, ErrorOr<RegistrationResultDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ISecurity securityService;
    private readonly IRoleRepository roleRepository;
    private readonly IUserRepository userRepository;
    private readonly ISetupRepository setupRepository;
    private readonly IQRCodeGenerator qrCodeGenerator;
    private readonly ITotpTokenGenerator totpTokenGenerator;
    private readonly IUserRoleRepository userRoleRepository;
    private readonly IPermissionRepository permissionRepository;
    private readonly IUserPreferenceRepository userPreferenceRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="totpTokenGenerator">Injected service for generating TOTP tokens</param>
    /// <param name="qrCodeGenerator">Injected service for creating QR codes</param>
    /// <param name="securityService">Injected service for security related functionality</param>
    public SetupCommandHandler(IUnitOfWork unitOfWork, ITotpTokenGenerator totpTokenGenerator, IQRCodeGenerator qrCodeGenerator, ISecurity securityService)
    {
        this.securityService = securityService;
        this.qrCodeGenerator = qrCodeGenerator;
        this.totpTokenGenerator = totpTokenGenerator;
        userRepository = unitOfWork.GetRepository<IUserRepository>();
        roleRepository = unitOfWork.GetRepository<IRoleRepository>();
        setupRepository = unitOfWork.GetRepository<ISetupRepository>();
        userRoleRepository = unitOfWork.GetRepository<IUserRoleRepository>();
        permissionRepository = unitOfWork.GetRepository<IPermissionRepository>();
        userPreferenceRepository = unitOfWork.GetRepository<IUserPreferenceRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Registers the admin account in the repository
    /// </summary>
    /// <param name="command">The admin account to be registered</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>A DTO containing the admin register result, or an error</returns>
    public async Task<ErrorOr<RegistrationResultDto>> Handle(SetupCommand command, CancellationToken cancellationToken)
    {
        // check if any users already exists (admin account is only set once!)
        var resultSelectUser = await userRepository.GetAllAsync();
        if (resultSelectUser.Data is not null)
            return Errors.Authorization.InvalidPermissionError;
        // run the database setup 
        var resultDatabaseSetup = await setupRepository.SetDatabase();
        if (resultDatabaseSetup.Error is not null)
            return Errors.DataAccess.DatabaseSetupError;
        // no users are present, register the admin one
        string? totpSecret = null;
        var user = new UserDto
        {
            Email = command.Email,
            Password = Uri.EscapeDataString(securityService.HashService.HashString(command.Password)),
            LastName = command.LastName,
            FirstName = command.FirstName,
        };
        // if the user enabled two factor auth, include a QR with the totp secret
        if (command.Use2fa)
        {
            // generate a TOTP secret
            byte[] secret = totpTokenGenerator.GenerateSecret();
            // convert the secret into a QR code for the user to scan
            totpSecret = qrCodeGenerator.GenerateQrCodeDataUri(command.Email, secret);
            // store the TOTP secret in the repository, encrypted
            user.TotpSecret = securityService.CryptographyService.Encrypt(Convert.ToBase64String(secret));
        }
        // insert the user
        var resultInsertUser = await userRepository.InsertAsync(user.ToStorageDto());
        if (resultInsertUser.Error is not null || resultInsertUser.Data is null)
            return Errors.DataAccess.InsertUserError;
        // create the default permissions
        var addPermissionResult = await AddPermissions();
        if (addPermissionResult.IsError)
            return addPermissionResult.Errors;
        // add the "Admin" role
        var resultAddAdmin = await AddAdminRole(addPermissionResult.Value, resultInsertUser.Data[0].Id);
        if (resultAddAdmin.IsError)
            return resultAddAdmin.Errors;
        // insert the default admin profile preferences
        var resultAddAdminPreferences = await AddAdminProfilePreferences(resultInsertUser.Data[0].Id, command.Use2fa);
        if (resultAddAdminPreferences.IsError)
            return resultAddAdminPreferences.Errors;
        user.Password = string.Empty; // do not include the password in the returned result!
        user.TotpSecret = totpSecret; // if 2FA was enabled, the TOTP secret needs to be delivered to the client unhashed, so it can be displayed 
        return new RegistrationResultDto(user);
    }

    /// <summary>
    /// Adds the default permissions into the repository
    /// </summary>
    /// <returns>A collection of permission ids, or an error</returns>
    private async Task<ErrorOr<List<int>>> AddPermissions()
    {
        // create the default permissions
        string? permissionInsertError = (await permissionRepository.InsertAsync(new PermissionDto() { PermissionName = "CanViewUsers" }.ToStorageDto()))?.Error;
        permissionInsertError = (await permissionRepository.InsertAsync(new PermissionDto() { PermissionName = "CanEditUsers" }.ToStorageDto()))?.Error ?? permissionInsertError;
        permissionInsertError = (await permissionRepository.InsertAsync(new PermissionDto() { PermissionName = "CanViewPermissions" }.ToStorageDto()))?.Error ?? permissionInsertError;
        permissionInsertError = (await permissionRepository.InsertAsync(new PermissionDto() { PermissionName = "CanViewSettings" }.ToStorageDto()))?.Error ?? permissionInsertError;
        if (permissionInsertError is not null)
            return Errors.DataAccess.InsertPermissionError;
        // get the list of permissions that were just added
        var resultGetPermissions = await permissionRepository.GetAllAsync();
        if (resultGetPermissions.Error is not null || resultGetPermissions.Data is null)
            return Errors.DataAccess.GetPermissionsError;
        return resultGetPermissions.Data!.Select(p => p.Id).ToList();
    }

    /// <summary>
    /// Adds the admin role
    /// </summary>
    /// <param name="permissionIds">The list of permissions to assign to the admin role</param>
    /// <param name="userId">The id of the user to assign the role of admin to</param>
    /// <returns><see langword="true"/>, or an error</returns>
    private async Task<ErrorOr<bool>> AddAdminRole(List<int> permissionIds, int userId)
    {
        // add the "Admin" role and assign all permissions to it
        var resultInsertAdminRole = await roleRepository.InsertAsync("Admin", permissionIds);
        if (resultInsertAdminRole.Error is not null || resultInsertAdminRole.Data is null)
            return Errors.DataAccess.InsertRoleError;
        // add the "Admin" role to the user
        var resultUpdateUserRole = await userRoleRepository.InsertAsync(new UserRoleDto() { RoleId = resultInsertAdminRole.Data[0].Id, UserId = userId }.ToStorageDto());
        if (resultUpdateUserRole.Error is not null || resultUpdateUserRole.Data is null)
            return Errors.DataAccess.UpdateUserRoleError;
        return true;
    }

    /// <summary>
    /// Adds the admin role
    /// </summary>
    /// <param name="userId">The id of the user whose profile preferences are added</param>
    /// <param name="use2fa">Whether the user enabled 2fa or not when registering the account</param>
    /// <returns><see langword="true"/>, or an error</returns>
    private async Task<ErrorOr<bool>> AddAdminProfilePreferences(int userId, bool use2fa)
    {
        ProfilePreferencesDto preferences = new()
        {
            Use2fa = use2fa,
            FullImageQuality = 90,
            ImagePreviewsQuality = 70,
            ThumbnailsRetrievalBatchSize = 20,
            ScrollThumbnailRetrievalTimeout = 1000,
            EnableConsoleDebugMessages = false,
            InspectFileForThumbnails = false,
            RememberOpenTabs = true,
            ShowImagePreviews = true,
            UserId = userId
        };
        var resultInsertAdminPreferences = await userPreferenceRepository.InsertAsync(preferences.ToStorageDto());
        if (resultInsertAdminPreferences.Error is not null || resultInsertAdminPreferences.Data is null)
            return Errors.DataAccess.InsertUserPreferencesError;
        return true;
    }
    #endregion
}