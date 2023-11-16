#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using Mapster;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Application.Common.DTO.FileSystem;
using Lyrida.Domain.Core.FileSystem.Services.Paths;
using Lyrida.Domain.Core.FileSystem.Services.Directories;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Commands.Create;

/// <summary>
/// Create directory command handler
/// </summary>
/// <remarks>
/// Creation Date: 11th of November, 2023
/// </remarks>
public class CreateDirectoryCommandHandler : IRequestHandler<CreateDirectoryCommand, ErrorOr<DirectoryDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IPathService pathService;
    private readonly IDirectoryService directoryService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="directoryService">Injected service for directories related functionality.</param>
    /// <param name="pathService">Injected service for paths related functionality.</param>
    public CreateDirectoryCommandHandler(IDirectoryService directoryService, IPathService pathService)
    {
        this.pathService = pathService;
        this.directoryService = directoryService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates a directory with the specified name, at the specified path.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a new directory result, or an error.</returns>
    public Task<ErrorOr<DirectoryDto>> Handle(CreateDirectoryCommand command, CancellationToken cancellationToken)
    {
        // make sure the path is in the expected format
        string path = command.Path;
        if (!path.EndsWith(pathService.PathSeparator))
            path += pathService.PathSeparator;
        // create the new directory
        ErrorOr<Directory> renameFileResult = directoryService.CreateDirectory(path, command.Name);
        return Task.FromResult(renameFileResult.Match(values => ErrorOrFactory.From(values.Adapt<DirectoryDto>()), errors => errors));
    }
    #endregion
}