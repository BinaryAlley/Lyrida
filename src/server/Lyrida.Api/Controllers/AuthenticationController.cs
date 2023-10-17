#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using ErrorOr;
using MapsterMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Localization;
using Lyrida.Application.Common.Errors.Types;
using Lyrida.Api.Common.Entities.Authentication;
using Lyrida.Application.Common.Entities.Common;
using Lyrida.Application.Common.Entities.Authentication;
using Lyrida.Application.Core.Authentication.Queries.Login;
using Lyrida.Application.Core.Authentication.Commands.Register;
using Lyrida.Application.Core.Authentication.Queries.VerifyReset;
using Lyrida.Application.Core.Authentication.Commands.ResetPassword;
using Lyrida.Application.Core.Authentication.Commands.ChangePassword;
using Lyrida.Application.Core.Authentication.Commands.RecoverPassword;
using Lyrida.Application.Core.Authentication.Queries.VerifyRegistration;
using Lyrida.Application.Core.Authentication.Commands.GenerateTotpQr;
#endregion

namespace Lyrida.Api.Controllers;

/// <summary>
/// Controller for managing User Authentication actions
/// </summary>
/// <remarks>
/// Creation Date: 07th of July, 2023
/// </remarks>
[AllowAnonymous]
[Route("authentication")]
public class AuthenticationController : ApiController
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IMapper mapper;
    private readonly ISender mediator;
    private readonly ITranslationService translationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="mediator">Injected service for mediating command and queries</param>
    /// <param name="translationService">Injected service for localization</param>
    /// <param name="mapper">Injected service for mapping objects</param>
    public AuthenticationController(ISender mediator, ITranslationService translationService, IMapper mapper) : base(translationService)
    {
        this.mapper = mapper;
        this.mediator = mediator;
        this.translationService = translationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Registers a new user identified by <paramref name="entity"/>
    /// </summary>
    /// <param name="entity"></param>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestEntity entity)
    {
        ErrorOr<RegistrationResultEntity> registerResult = await mediator.Send(mapper.Map<RegisterCommand>(entity));
        return registerResult.Match(result => Ok(mapper.Map<AuthenticationResponseEntity>(result)), errors => Problem(errors));
    }

    /// <summary>
    /// Authenticates an user identified by <paramref name="entity"/>
    /// </summary>
    /// <param name="entity">The entity used for the login</param>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestEntity entity)
    {
        ErrorOr<AuthenticationResultEntity> authResult = await mediator.Send(mapper.Map<LoginQuery>(entity));
        // handle the special case when the provided credentials were not ok
        if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidUsername)
        {
            string title = string.Empty;
            if (Enum.TryParse<Terms>(authResult.FirstError.Code, out var term)) // if there is a translation available for the error term, use it
                title = translationService.Translate(term);
            else
                title = authResult.FirstError.Code;
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: title);
        }
        return authResult.Match(result => Ok(mapper.Map<AuthenticationResponseEntity>(result)), errors => Problem(errors));
    }

    /// Verifies <paramref name="token"/> for validity. If valid, the account owning the token is marked as active.
    /// </summary>
    /// <param name="token">The token to be verified</param>
    [HttpPost("verifyRegister")]
    public async Task<IActionResult> VerifyRegister(ValidateTokenRequestEntity token)
    {
        ErrorOr<AuthenticationResultEntity> verifyResult = await mediator.Send(mapper.Map<VerifyRegistrationTokenQuery>(token));
        return verifyResult.Match(result => Ok(mapper.Map<AuthenticationResponseEntity>(result)), errors => Problem(errors));
    }

    /// Verifies <paramref name="token"/> for validity. If valid, the account owning the token can reset its password.
    /// </summary>
    /// <param name="token">The token to be verified</param>
    [HttpPost("verifyReset")]
    public async Task<IActionResult> VerifyReset(ValidateTokenRequestEntity token)
    {
        ErrorOr<AuthenticationResultEntity> verifyResult = await mediator.Send(mapper.Map<VerifyResetTokenQuery>(token));
        return verifyResult.Match(authResult => Ok(mapper.Map<AuthenticationResponseEntity>(authResult)), errors => Problem(errors));
    }

    /// <summary>
    /// Sends a password recovery link to an account identified by <paramref name="email"/>.
    /// </summary>
    /// <param name="email">The email of the account for which to send a password recovery link</param>
    [HttpPost("recoverPassword")]
    public async Task<IActionResult> RecoverPassword(RecoverPasswordRequestEntity email)
    {
        ErrorOr<CommandResultEntity> recoverPasswordResult = await mediator.Send(mapper.Map<RecoverPasswordCommand>(email));
        return recoverPasswordResult.Match(result => NoContent(), errors => Problem(errors));
    }

    /// <summary>
    /// Sends a password recovery link to an account identified by <paramref name="entity"/>.
    /// </summary>
    /// <param name="entity">The email of the account for which to send a password recovery link</param>
    [Authorize]
    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequestEntity entity)
    {
        ErrorOr<RegistrationResultEntity> recoverPasswordResult = await mediator.Send(mapper.Map<ResetPasswordCommand>(entity));
        return recoverPasswordResult.Match(result => NoContent(), errors => Problem(errors));
    }

    /// <summary>
    /// Sends a password recovery link to an account identified by <paramref name="entity"/>.
    /// </summary>
    /// <param name="entity">The email of the account for which to send a password recovery link</param>
    [Authorize]
    [HttpPost("changePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequestEntity entity)
    {
        ErrorOr<RegistrationResultEntity> recoverPasswordResult = await mediator.Send(mapper.Map<ChangePasswordCommand>(entity));
        return recoverPasswordResult.Match(result => NoContent(), errors => Problem(errors));
    }

    /// <summary>
    /// Generates a QR code for TOTP authentication setup.
    /// </summary>
    [Authorize] // User should be authenticated to enable 2FA
    [HttpPost("generateTOTPQR")]
    public async Task<IActionResult> GenerateTOTPQR()
    {
        ErrorOr<QrCodeResultEntity> qrResult = await mediator.Send(new GenerateTotpQrCommand(User.Identity.Name)); // Assuming the username can be identified from the user claims.
        return qrResult.Match(result => Ok(result), errors => Problem(errors)); // QrCodeResultEntity should contain the QR code or URL.
    }

    ///// <summary>
    ///// Verifies the provided TOTP from user's authenticator app.
    ///// </summary>
    ///// <param name="entity">Entity containing the user's TOTP.</param>
    //[Authorize]
    //[HttpPost("verifyTOTP")]
    //public async Task<IActionResult> VerifyTOTP(VerifyTOTPRequestEntity entity)
    //{
    //    ErrorOr<CommandResultEntity> verifyResult = await mediator.Send(mapper.Map<VerifyTOTPCommand>(entity));
    //    return verifyResult.Match(result => NoContent(), errors => Problem(errors));
    //}

    ///// <summary>
    ///// Authenticates an user with TOTP after password authentication.
    ///// </summary>
    ///// <param name="entity">The entity containing the TOTP.</param>
    //[Authorize]
    //[HttpPost("loginWithTOTP")]
    //public async Task<IActionResult> LoginWithTOTP(LoginWithTOTPRequestEntity entity)
    //{
    //    ErrorOr<AuthenticationResultEntity> authResult = await mediator.Send(mapper.Map<LoginWithTOTPQuery>(entity));
    //    return authResult.Match(result => Ok(mapper.Map<AuthenticationResponseEntity>(result)), errors => Problem(errors));
    //}
    #endregion
}