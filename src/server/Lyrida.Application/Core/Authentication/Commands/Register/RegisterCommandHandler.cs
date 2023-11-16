#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.Infrastructure.Common.Time;
using Lyrida.DataAccess.Repositories.Users;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Infrastructure.Core.Authentication;
using Lyrida.Application.Common.DTO.Configuration;
using Lyrida.DataAccess.Repositories.Configuration;
using Lyrida.Application.Common.DTO.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.Register;

/// <summary>
/// Authentication register command handler
/// </summary>
/// <remarks>
/// Creation Date: 18th of July, 2023
/// </remarks>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<RegistrationResultDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ISecurity securityService;
    private readonly IUserRepository userRepository;
    private readonly IQRCodeGenerator qrCodeGenerator;
    private readonly ITotpTokenGenerator totpTokenGenerator;
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
    public RegisterCommandHandler(IUnitOfWork unitOfWork, ITotpTokenGenerator totpTokenGenerator, IQRCodeGenerator qrCodeGenerator, ISecurity securityService)
    {
        this.qrCodeGenerator = qrCodeGenerator;
        this.securityService = securityService;
        this.totpTokenGenerator = totpTokenGenerator;
        userRepository = unitOfWork.GetRepository<IUserRepository>();
        userPreferenceRepository = unitOfWork.GetRepository<IUserPreferenceRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Registers a new account in the repository
    /// </summary>
    /// <param name="command">The account to be registered</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>A DTO containing the register result, or an error</returns>
    public async Task<ErrorOr<RegistrationResultDto>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // check if the user already exists
        var resultSelectUser = await userRepository.GetByEmailAsync(command.Email);
        if (resultSelectUser.Error is null)
        {
            if (resultSelectUser.Data is not null)
                return Errors.Authentication.DuplicateEmailError;
        }
        else
            return Errors.DataAccess.GetUserError;
        string? totpSecret = null;
        var user = new UserDto
        {
            Email = command.Email,
            Password = Uri.EscapeDataString(securityService.HashService.HashString(command.Password)),
            LastName = command.LastName,
            FirstName = command.FirstName
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
        if (!string.IsNullOrEmpty(resultInsertUser.Error) || resultInsertUser.Data is null)
            return Errors.DataAccess.InsertUserError;
        // insert the default profile preferences
        var resultAddPreferences = await AddProfilePreferences(resultInsertUser.Data[0].Id, command.Use2fa);
        if (resultAddPreferences.IsError)
            return resultAddPreferences.Errors;
        user.Password = string.Empty; // do not include the password in the returned result!
        user.TotpSecret = totpSecret; // if 2FA was enabled, the TOTP secret needs to be delivered to the client unhashed, so it can be displayed 
        return new RegistrationResultDto(user);
    }

    /// <summary>
    /// Adds the admin role
    /// </summary>
    /// <param name="userId">The id of the user whose profile preferences are added</param>
    /// <param name="use2fa">Whether the user enabled 2fa or not when registering the account</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a boolean result, or an error.</returns>
    private async Task<ErrorOr<bool>> AddProfilePreferences(int userId, bool use2fa)
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
        var resultInsertPreferences = await userPreferenceRepository.InsertAsync(preferences.ToStorageDto());
        if (resultInsertPreferences.Error is not null || resultInsertPreferences.Data is null)
            return Errors.DataAccess.InsertUserPreferencesError;
        return true;
    }
    #endregion
}