#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using ErrorOr;
using MapsterMapper;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Lyrida.Domain.Common.Errors;
using Microsoft.AspNetCore.Authorization;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Localization;
using Lyrida.Api.Common.DTO.Authentication;
using Lyrida.Application.Common.DTO.Common;
using Lyrida.Application.Common.DTO.Authentication;
using Lyrida.Application.Core.Authentication.Queries.Login;
using Lyrida.Application.Core.Authentication.Commands.Register;
using Lyrida.Application.Core.Authentication.Commands.ChangePassword;
using Lyrida.Application.Core.Authentication.Commands.GenerateTotpQr;
using Lyrida.Application.Core.Authentication.Commands.RecoverPassword;
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
    /// Registers a new user identified by <paramref name="data"/>
    /// </summary>
    /// <param name="data">The data of the user to register</param>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto data)
    {
        ErrorOr<RegistrationResultDto> registerResult = await mediator.Send(mapper.Map<RegisterCommand>(data));
        return registerResult.Match(result => Ok(mapper.Map<AuthenticationResponseDto>(result)), errors => Problem(errors));
    }

    /// <summary>
    /// Authenticates an user identified by <paramref name="data"/>
    /// </summary>
    /// <param name="data">The data used for the login</param>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto data)
    {
        ErrorOr<AuthenticationResultDto> authResult = await mediator.Send(mapper.Map<LoginQuery>(data));
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
        return authResult.Match(result => Ok(mapper.Map<LoginResponseDto>(result)), errors => Problem(errors));
    }

    /// <summary>
    /// Sends a password recovery link to an account identified by <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The email of the account for which to send a password recovery link</param>
    [HttpPost("recoverPassword")]
    public async Task<IActionResult> RecoverPassword(RecoverPasswordRequestDto data)
    {
        ErrorOr<CommandResultDto> recoverPasswordResult = await mediator.Send(mapper.Map<RecoverPasswordCommand>(data));
        return recoverPasswordResult.Match(result => NoContent(), errors => Problem(errors));
    }

    /// <summary>
    /// Sends a password recovery link to an account identified by <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The email of the account for which to send a password recovery link</param>
    [Authorize]
    [HttpPost("changePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto data)
    {
        ErrorOr<RegistrationResultDto> recoverPasswordResult = await mediator.Send(mapper.Map<ChangePasswordCommand>(data));
        return recoverPasswordResult.Match(result => NoContent(), errors => Problem(errors));
    }

    /// <summary>
    /// Generates a QR code for TOTP authentication setup.
    /// </summary>
    [Authorize] // User should be authenticated to enable 2FA
    [HttpGet("generateTOTPQR")]
    public async Task<IActionResult> GenerateTOTPQR()
    {
        if (!TryGetUserId(out int userId))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: translationService.Translate(Terms.InvalidUserId));
        ErrorOr<QrCodeResultDto> qrResult = await mediator.Send(new GenerateTotpQrCommand(userId)); 
        return qrResult.Match(result => Ok(result), errors => Problem(errors)); 
    }

    /// <summary>
    /// Gets the id of the user currently making requests
    /// </summary>
    /// <param name="userId">The id of the user currently making requests</param>
    /// <returns>True if the id of the user currently making requests could be parsed, False otherwise</returns>
    private bool TryGetUserId(out int userId)
    {
        var userIdClaim = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out userId);
    }
    #endregion
}