#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using MapsterMapper;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Localization;
using Lyrida.Api.Common.DTO.Configuration;
using Lyrida.Application.Common.DTO.Configuration;
using Lyrida.Application.Core.Configuration.Queries.Read;
using Lyrida.Application.Core.Configuration.Commands.Update;
#endregion

namespace Lyrida.Api.Controllers;

/// <summary>
/// Controller for managing User Authentication actions
/// </summary>
/// <remarks>
/// Creation Date: 07th of July, 2023
/// </remarks>
[Route("account")]
public class AccountController : ApiController
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
    /// <param name="mapper">Injected service for mapping objects</param>
    /// <param name="translationService">Injected service for localization</param>
    public AccountController(ISender mediator, IMapper mapper, ITranslationService translationService) : base(translationService)
    {
        this.mapper = mapper;
        this.mediator = mediator;
        this.translationService = translationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Updates the profile preferences of a user
    /// </summary>
    /// <param name="data">The profile preferences of the user</param>
    [HttpPut("updatePreferences")]
    public async Task<IActionResult> UpdatePreferences(ProfilePreferenceRequestDto data)
    {
        if (!TryGetUserId(out int userId))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: translationService.Translate(Terms.InvalidUserId));
        ErrorOr<ProfilePreferencesDto> registerResult = await mediator.Send(new UpdateProfilePreferenceCommand(userId, mapper.Map<ProfilePreferencesDto>(data)));
        return registerResult.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Gets the directories of <paramref name="path"/>
    /// </summary>
    [HttpGet("getPreferences")]
    public async Task<IActionResult> GetPreferences()
    {
        if (!TryGetUserId(out int userId))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: translationService.Translate(Terms.InvalidUserId));
        ErrorOr<ProfilePreferencesDto> getResult = await mediator.Send(new GetProfilePreferencesQuery(userId));
        return getResult.Match(result => Ok(result), errors => Problem(errors));
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