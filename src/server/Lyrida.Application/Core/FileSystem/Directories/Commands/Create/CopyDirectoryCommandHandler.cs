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

namespace Lyrida.Application.Core.FileSystem.Directories.Commands.Create;

/// <summary>
/// Copy directory command handler
/// </summary>
/// <remarks>
/// Creation Date: 14th of November, 2023
/// </remarks>
public class CopyDirectoryCommandHandler : IRequestHandler<CopyDirectoryCommand, ErrorOr<DirectoryDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDirectoryService directoryService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="directoryService">Injected service for directories related functionality.</param>
    public CopyDirectoryCommandHandler(IDirectoryService directoryService)
    {
        this.directoryService = directoryService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Copies a directory.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a copied directory result, or an error.</returns>
    public Task<ErrorOr<DirectoryDto>> Handle(CopyDirectoryCommand command, CancellationToken cancellationToken)
    {
        // copy the directory
        ErrorOr<Directory> copyDirectoryResult = directoryService.CopyDirectory(command.SourcePath, command.DestinationPath, command.OverrideExisting);
        return Task.FromResult(copyDirectoryResult.Match(values => ErrorOrFactory.From(values.Adapt<DirectoryDto>()), errors => errors));
    }
    #endregion
}