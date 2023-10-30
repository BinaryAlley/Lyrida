#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Lyrida.Api.Common.ModelBinders;
using Lyrida.Infrastructure.Localization;
using Microsoft.AspNetCore.Authorization;
using Lyrida.Application.Core.FileSystem.Paths.Queries.Read;
using Lyrida.Application.Common.DTO.FileSystem;
#endregion

namespace Lyrida.Api.Controllers;

/// <summary>
/// Controller for managing paths actions
/// </summary>
/// <remarks>
/// Creation Date: 02nd of October, 2023
/// </remarks>
[Route("[controller]")]
public class PathsController : ApiController
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ISender mediator;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="mediator">Injected service for mediating command and queries.</param>
    /// <param name="translationService">Injected service for localization.</param>
    public PathsController(ISender mediator, ITranslationService translationService) : base(translationService)
    {
        this.mediator = mediator;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Validates the validity of <paramref name="path"/>.
    /// </summary>
    [HttpGet("validate")]
    public async Task<IActionResult> ValidatePath([FromQuery, ModelBinder(typeof(UrlStringBinder))] string path)
    {
        return Ok(await mediator.Send(new ValidatePathQuery(path)));
    }

    /// <summary>
    /// Validates the validity of <paramref name="path"/>.
    /// </summary>
    [HttpGet("parse")]
    public async Task<IActionResult> ParsePath([FromQuery, ModelBinder(typeof(UrlStringBinder))] string path)
    {
        ErrorOr<IEnumerable<PathSegmentDto>> getResult = await mediator.Send(new ParsePathQuery(path));
        return getResult.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Navigates up one level from <paramref name="path"/>.
    /// </summary>
    [HttpGet("goUpOneLevel")]
    public async Task<IActionResult> GoUpOneLevel([FromQuery, ModelBinder(typeof(UrlStringBinder))] string path)
    {
        ErrorOr<IEnumerable<PathSegmentDto>> getResult = await mediator.Send(new NavigateUpOneLevelQuery(path));
        return getResult.Match(result => Ok(result), errors => Problem(errors));
    }
    #endregion
}