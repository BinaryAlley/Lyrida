#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using Mapster;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Application.Common.DTO.FileSystem;
using Lyrida.Domain.Core.FileSystem.Services.Paths;
using Lyrida.Domain.Core.FileSystem.Services.Directories;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Commands.Update;

/// <summary>
/// Move directory command handler
/// </summary>
/// <remarks>
/// Creation Date: 14th of November, 2023
/// </remarks>
public class MoveDirectoryCommandHandler : IRequestHandler<MoveDirectoryCommand, ErrorOr<DirectoryDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDirectoryService directoryService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="directoryService">Injected service for directories related functionality.</param>
    public MoveDirectoryCommandHandler(IDirectoryService directoryService)
    {
        this.directoryService = directoryService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Moves a directory.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a moved directory result, or an error.</returns>
    public Task<ErrorOr<DirectoryDto>> Handle(MoveDirectoryCommand command, CancellationToken cancellationToken)
    {
        ErrorOr<Directory> renameFileResult = directoryService.MoveDirectory(command.SourcePath, command.DestinationPath, command.OverrideExisting);
        return Task.FromResult(renameFileResult.Match(values => ErrorOrFactory.From(values.Adapt<DirectoryDto>()), errors => errors));
    }
    #endregion
}