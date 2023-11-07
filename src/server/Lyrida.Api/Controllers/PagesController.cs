#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using MapsterMapper;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Lyrida.Api.Common.DTO.Pages;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Localization;
using Lyrida.Application.Common.DTO.Pages;
using Lyrida.Application.Core.Pages.Queries.Read;
using Lyrida.Application.Core.Pages.Commands.Create;
using Lyrida.Application.Core.Pages.Commands.Delete;
using Lyrida.Application.Core.Pages.Commands.Update;
#endregion

namespace Lyrida.Api.Controllers;

/// <summary>
/// Controller for managing User Authentication actions
/// </summary>
/// <remarks>
/// Creation Date: 07th of July, 2023
/// </remarks>
[Route("pages")]
public class PagesController : ApiController
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
    public PagesController(ISender mediator, IMapper mapper, ITranslationService translationService) : base(translationService)
    {
        this.mapper = mapper;
        this.mediator = mediator;
        this.translationService = translationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Adds a page to the collection of open pages of the current user.
    /// </summary>
    /// <param name="data">The page to be added.</param>
    [HttpPost()]
    public async Task<IActionResult> AddPage([FromBody] PageRequestDto data)
    {
        if (!TryGetUserId(out int userId))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: translationService.Translate(Terms.InvalidUserId));
        ErrorOr<PageDto> registerResult = await mediator.Send(new AddPageCommand(userId, mapper.Map<PageDto>(data)));
        return registerResult.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Gets the collection of open user pages of the current user.
    /// </summary>
    [HttpGet()]
    public async Task<IActionResult> GetPages()
    {
        if (!TryGetUserId(out int userId))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: translationService.Translate(Terms.InvalidUserId));
        ErrorOr<IEnumerable<PageDto>> getResult = await mediator.Send(new GetPagesQuery(userId));
        return getResult.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Deletes a user page identified by <paramref name="guid"/>.
    /// </summary>
    [HttpDelete("{guid}")]
    public async Task<IActionResult> Delete(string guid)
    {
        if (!TryGetUserId(out int userId))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: translationService.Translate(Terms.InvalidUserId));
        ErrorOr<bool> result = await mediator.Send(new DeletePageCommand(userId, guid));
        return result.Match(result => NoContent(), errors => Problem(errors));
    }

    /// <summary>
    /// Updates a page in the collection of open user pages of the current user.
    /// </summary>
    [HttpPut()]
    public async Task<IActionResult> UpdatePage([FromBody] PageRequestDto data)
    {
        if (!TryGetUserId(out int userId))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: translationService.Translate(Terms.InvalidUserId));
        ErrorOr<bool> updateResult = await mediator.Send(new UpdatePageCommand(userId, mapper.Map<PageDto>(data)));
        return updateResult.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Gets the id of the user currently making requests.
    /// </summary>
    /// <param name="userId">The id of the user currently making requests.</param>
    /// <returns>True if the id of the user currently making requests could be parsed, False otherwise.</returns>
    private bool TryGetUserId(out int userId)
    {
        var userIdClaim = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out userId);
    }
    #endregion
}
