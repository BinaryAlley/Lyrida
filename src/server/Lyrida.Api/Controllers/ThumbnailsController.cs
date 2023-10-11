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
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the thumbnail of a file identified by of <paramref name="path"/>
    /// </summary>
    [HttpGet()]
    [AllowAnonymous]
    public async Task<IActionResult> GetThumbnail([FromQuery, ModelBinder(typeof(UrlStringBinder))] string path)
    {
        ErrorOr<ThumbnailDto> getResult = await mediator.Send(new GetThumbnailQuery(path));
        return getResult.Match(result => File(result.Bytes, MimeTypes.GetMimeType(result.Type)), errors => Problem(errors));
    }
    #endregion
}