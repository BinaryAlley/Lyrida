#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using MapsterMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Lyrida.Api.Common.ModelBinders;
using Lyrida.Infrastructure.Localization;
using Lyrida.Application.Common.DTO.FileSystem;
using Lyrida.Application.Core.FileSystem.Files.Queries.Read;
using Lyrida.Application.Core.FileSystem.Files.Commands.Delete;
using Lyrida.Application.Core.FileSystem.Files.Commands.Update;
using Lyrida.Application.Core.FileSystem.Files.Commands.Create;
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
    private readonly IMapper mapper;
    private readonly ISender mediator;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="mediator">Injected service for mediating command and queries</param>
    /// <param name="translationService">Injected service for localization</param>
    /// <param name="mapper">Injected service for mapping objects</param>
    public FilesController(ISender mediator, ITranslationService translationService, IMapper mapper) : base(translationService)
    {
        this.mapper = mapper;
        this.mediator = mediator;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the directories of <paramref name="path"/>.
    /// </summary>
    [HttpGet()]
    public async Task<IActionResult> GetFiles([FromQuery, ModelBinder(typeof(UrlStringBinder))] string path)
    {
        ErrorOr<IEnumerable<FileDto>> result = await mediator.Send(new GetFilesQuery(path));
        return result.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Renames a file.
    /// </summary>
    /// <param name="data">The DTO containing the data needed to rename the file.</param>
    [HttpPut()]
    public async Task<IActionResult> RenameFile([FromBody] FileSystemItemDto data)
    {
        ErrorOr<FileDto> result = await mediator.Send(mapper.Map<RenameFileCommand>(data));
        return result.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Copies a file.
    /// </summary>
    /// <param name="data">The DTO containing the data needed to copy the file.</param>
    [HttpPost("Copy")]
    public async Task<IActionResult> CopyFile([FromBody] PasteFileSystemItemDto data)
    {
        ErrorOr<FileDto> result = await mediator.Send(mapper.Map<CopyFileCommand>(data));
        return result.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Moves a file.
    /// </summary>
    /// <param name="data">The DTO containing the data needed to move the file.</param>
    [HttpPut("Move")]
    public async Task<IActionResult> MoveFile([FromBody] PasteFileSystemItemDto data)
    {
        ErrorOr<FileDto> result = await mediator.Send(mapper.Map<MoveFileCommand>(data));
        return result.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Deletes the file at <paramref name="path"/>.
    /// </summary>
    [HttpDelete()]
    public async Task<IActionResult> DeleteFile([FromQuery, ModelBinder(typeof(UrlStringBinder))] string path)
    {
        ErrorOr<bool> result = await mediator.Send(new DeleteFileCommand(path));
        return result.Match(result => Ok(result), errors => Problem(errors));
    }
    #endregion
}