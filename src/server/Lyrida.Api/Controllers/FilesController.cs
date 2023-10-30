#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using MapsterMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Lyrida.Api.Common.ModelBinders;
using Lyrida.Infrastructure.Localization;
using Microsoft.AspNetCore.Authorization;
using Lyrida.Application.Core.FileSystem.Files.Queries.Read;
using Lyrida.Application.Common.DTO.FileSystem;
#endregion

namespace Lyrida.Api.Controllers;

/// <summary>
/// Controller for managing files actions
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
[Route("[controller]")]
public class FilesController : ApiController
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
    public FilesController(ISender mediator, ITranslationService translationService) : base(translationService)
    {
        this.mediator = mediator;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the directories of <paramref name="path"/>
    /// </summary>
    [HttpGet()]
    public async Task<IActionResult> GetFiles([FromQuery, ModelBinder(typeof(UrlStringBinder))] string path)
    {
        ErrorOr<IEnumerable<FileDto>> getResult = await mediator.Send(new GetFilesQuery(path));
        return getResult.Match(result => Ok(result), errors => Problem(errors));
    }
    #endregion
}