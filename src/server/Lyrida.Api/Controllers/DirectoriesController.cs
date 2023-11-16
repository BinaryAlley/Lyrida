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
using Lyrida.Application.Common.DTO.FileSystem;
using Lyrida.Application.Core.FileSystem.Directories.Queries.Read;
using Lyrida.Application.Core.FileSystem.Directories.Commands.Delete;
using Lyrida.Application.Core.FileSystem.Directories.Commands.Create;
using Lyrida.Application.Core.FileSystem.Directories.Commands.Update;
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
    private readonly IMapper mapper;
    private readonly ISender mediator;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="mediator">Injected service for mediating commands and queries</param>
    /// <param name="translationService">Injected service for localization</param>
    /// <param name="mapper">Injected service for mapping objects</param>
    public DirectoriesController(ISender mediator, ITranslationService translationService, IMapper mapper) : base(translationService)
    {
        this.mapper = mapper;
        this.mediator = mediator;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the directories of <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path for which to get the directories.</param>
    [HttpGet()]
    public async Task<IActionResult> GetDirectories([FromQuery, ModelBinder(typeof(UrlStringBinder))] string path)
    {
        ErrorOr<IEnumerable<DirectoryDto>> result = await mediator.Send(new GetDirectoriesQuery(path));
        return result.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Creates a new directory.
    /// </summary>
    /// <param name="data">The DTO containing the data needed to create the new directory.</param>
    [HttpPost()]
    public async Task<IActionResult> CreateDirectory([FromBody] FileSystemItemDto data)
    {
        ErrorOr<DirectoryDto> result = await mediator.Send(mapper.Map<CreateDirectoryCommand>(data));
        return result.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Renames a directory.
    /// </summary>
    /// <param name="data">The DTO containing the data needed to rename the directory.</param>
    [HttpPut()]
    public async Task<IActionResult> RenameDirectory([FromBody] FileSystemItemDto data)
    {
        ErrorOr<DirectoryDto> result = await mediator.Send(mapper.Map<RenameDirectoryCommand>(data));
        return result.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Copies a directory.
    /// </summary>
    /// <param name="data">The DTO containing the data needed to copy the directory.</param>
    [HttpPost("Copy")]
    public async Task<IActionResult> CopyDirectory([FromBody] PasteFileSystemItemDto data)
    {
        ErrorOr<DirectoryDto> result = await mediator.Send(mapper.Map<CopyDirectoryCommand>(data));
        return result.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Moves a directory.
    /// </summary>
    /// <param name="data">The DTO containing the data needed to move the directory.</param>
    [HttpPut("Move")]
    public async Task<IActionResult> MoveDirectory([FromBody] PasteFileSystemItemDto data)
    {
        ErrorOr<DirectoryDto> result = await mediator.Send(mapper.Map<MoveDirectoryCommand>(data));
        return result.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Deletes the directory at <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path of the directory to delete.</param>
    [HttpDelete()]
    public async Task<IActionResult> DeleteDirectory([FromQuery, ModelBinder(typeof(UrlStringBinder))] string path)
    {
        ErrorOr<bool> result = await mediator.Send(new DeleteDirectoryCommand(path));
        return result.Match(result => Ok(result), errors => Problem(errors));
    }
    #endregion
}