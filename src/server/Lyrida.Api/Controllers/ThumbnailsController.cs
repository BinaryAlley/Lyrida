#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading.Tasks;
using Lyrida.Domain.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Lyrida.Api.Common.Utilities;
using Lyrida.Api.Common.ModelBinders;
using Lyrida.Infrastructure.Localization;
using Microsoft.AspNetCore.Authorization;
using Lyrida.Application.Core.FileSystem.Thumbnails.Queries.Read;
using System.Security.Claims;
using Lyrida.Infrastructure.Common.Enums;
using Microsoft.AspNetCore.Http;
#endregion

namespace Lyrida.Api.Controllers;

/// <summary>
/// Controller for managing thumbnails actions
/// </summary>
/// <remarks>
/// Creation Date: 28th of September, 2023
/// </remarks>
[Route("[controller]")]
public class ThumbnailsController : ApiController
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ISender mediator;
    private readonly ITranslationService translationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="mediator">Injected service for mediating command and queries</param>
    /// <param name="translationService">Injected service for localization</param>
    public ThumbnailsController(ISender mediator, ITranslationService translationService) : base(translationService)
    {
        this.mediator = mediator;
        this.translationService = translationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the thumbnail of a file identified by of <paramref name="path"/>
    /// </summary>
    /// <param name="path">The path of the file for which to get the thumbnail.</param>
    /// <param name="quality">The quality to use for the thumbnail.</param>
    [HttpGet()]
    public async Task<IActionResult> GetThumbnail([FromQuery, ModelBinder(typeof(UrlStringBinder))] string path, [FromQuery] int quality)
    {
        if (!TryGetUserId(out int userId))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: translationService.Translate(Terms.InvalidUserId));
        ErrorOr<ThumbnailDto> getResult = await mediator.Send(new GetThumbnailQuery(path, quality, userId));
        return getResult.Match(result => File(result.Bytes, MimeTypes.GetMimeType(result.Type)), errors => Problem(errors));
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