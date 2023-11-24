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
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Infrastructure.Core.Authentication;
using Lyrida.Application.Common.DTO.Configuration;
using Lyrida.Application.Common.DTO.Authentication;
using Lyrida.DataAccess.Repositories.Configuration;
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
    private readonly IUserRepository userRepository;
    private readonly ISetupRepository setupRepository;
    private readonly IQRCodeGenerator qrCodeGenerator;
    private readonly ITotpTokenGenerator totpTokenGenerator;
    private readonly IUserPreferenceRepository userPreferenceRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories.</param>
    /// <param name="totpTokenGenerator">Injected service for generating TOTP tokens.</param>
    /// <param name="qrCodeGenerator">Injected service for creating QR codes.</param>
    /// <param name="securityService">Injected service for security related functionality.</param>
    public SetupCommandHandler(IUnitOfWork unitOfWork, ITotpTokenGenerator totpTokenGenerator, IQRCodeGenerator qrCodeGenerator, ISecurity securityService)
    {
        this.securityService = securityService;
        this.qrCodeGenerator = qrCodeGenerator;
        this.totpTokenGenerator = totpTokenGenerator;
        userRepository = unitOfWork.GetRepository<IUserRepository>();
        setupRepository = unitOfWork.GetRepository<ISetupRepository>();
        userPreferenceRepository = unitOfWork.GetRepository<IUserPreferenceRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Registers the admin account in the repository.
    /// </summary>
    /// <param name="command">The admin account to be registered.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a DTO containing the admin register result, or an error.</returns>
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
            Username = command.Username,
            Password = Uri.EscapeDataString(securityService.HashService.HashString(command.Password)),
        };
        // if the user enabled two factor auth, include a QR with the totp secret
        if (command.Use2fa)
        {
            // generate a TOTP secret
            byte[] secret = totpTokenGenerator.GenerateSecret();
            // convert the secret into a QR code for the user to scan
            totpSecret = qrCodeGenerator.GenerateQrCodeDataUri(command.Username, secret);
            // store the TOTP secret in the repository, encrypted
            user.TotpSecret = securityService.CryptographyService.Encrypt(Convert.ToBase64String(secret));
        }
        // insert the user
        var resultInsertUser = await userRepository.InsertAsync(user.ToStorageDto());
        if (resultInsertUser.Error is not null || resultInsertUser.Data is null)
            return Errors.DataAccess.InsertUserError;
        // insert the default admin profile preferences
        var resultAddAdminPreferences = await AddAdminProfilePreferences(resultInsertUser.Data[0].Id, command.Use2fa);
        if (resultAddAdminPreferences.IsError)
            return resultAddAdminPreferences.Errors;
        user.Password = string.Empty; // do not include the password in the returned result!
        user.TotpSecret = totpSecret; // if 2FA was enabled, the TOTP secret needs to be delivered to the client unhashed, so it can be displayed 
        return new RegistrationResultDto(user);
    }

    /// <summary>
    /// Adds the admin account preferences.
    /// </summary>
    /// <param name="userId">The id of the user whose profile preferences are added.</param>
    /// <param name="use2fa">Whether the user enabled 2fa or not when registering the account.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a boolean result, or an error.</returns>
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