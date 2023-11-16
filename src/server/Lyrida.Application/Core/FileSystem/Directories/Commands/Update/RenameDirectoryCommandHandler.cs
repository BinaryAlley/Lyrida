#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using Mapster;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Application.Common.DTO.FileSystem;
using Lyrida.Domain.Core.FileSystem.Services.Directories;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Commands.Update;

/// <summary>
/// Rename directory command handler
/// </summary>
/// <remarks>
/// Creation Date: 13th of November, 2023
/// </remarks>
public class RenameDirectoryCommandHandler : IRequestHandler<RenameDirectoryCommand, ErrorOr<DirectoryDto?>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDirectoryService directoryService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="directoryService">Injected service for directories related functionality.</param>
    public RenameDirectoryCommandHandler(IDirectoryService directoryService)
    {
        this.directoryService = directoryService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Renames a directory with the specified name, at the specified path.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a new directory result, or an error.</returns>
    public Task<ErrorOr<DirectoryDto?>> Handle(RenameDirectoryCommand command, CancellationToken cancellationToken)
    {
        ErrorOr<Directory> renameFileResult = directoryService.RenameDirectory(command.Path, command.Name);
        return Task.FromResult(renameFileResult.Match(values => ErrorOrFactory.From(values.Adapt<DirectoryDto?>()), errors => errors));
    }
    #endregion
}