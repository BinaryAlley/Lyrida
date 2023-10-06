#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using MapsterMapper;
using Lyrida.Api.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Lyrida.Api.Common.ModelBinders;
using Lyrida.Infrastructure.Localization;
using Microsoft.AspNetCore.Authorization;
using Lyrida.Application.Common.Entities.FileSystem;
using Lyrida.Application.Core.FileSystem.Directories.Queries.Read;
#endregion

namespace Lyrida.API.Controllers;

/// <summary>
/// Controller for managing directories actions
/// </summary>
/// <remarks>
/// Creation Date: 22nd of September, 2023
/// </remarks>
[Route("[controller]")]
public class DirectoriesController : ApiController
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ISender mediator;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="mediator">Injected service for mediating commands and queries</param>
    /// <param name="translationService">Injected service for localization</param>
    public DirectoriesController(ISender mediator, ITranslationService translationService) : base(translationService)
    {
        this.mediator = mediator;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the directories of <paramref name="path"/>
    /// </summary>
    [HttpGet()]
    [AllowAnonymous]
    public async Task<IActionResult> GetDirectories([FromQuery, ModelBinder(typeof(UrlStringBinder))] string path)
    {
        ErrorOr<IEnumerable<DirectoryEntity>> getResult = await mediator.Send(new GetFoldersQuery(path));
        return getResult.Match(result => Ok(result), errors => Problem(errors));
    }
    #endregion
}