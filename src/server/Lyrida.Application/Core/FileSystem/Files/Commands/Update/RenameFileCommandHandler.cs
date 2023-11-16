#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using Mapster;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Application.Common.DTO.FileSystem;
using Lyrida.Domain.Core.FileSystem.Services.Paths;
using Lyrida.Domain.Core.FileSystem.Services.Files;
#endregion

namespace Lyrida.Application.Core.FileSystem.Files.Commands.Update;

/// <summary>
/// Rename file command handler
/// </summary>
/// <remarks>
/// Creation Date: 13th of November, 2023
/// </remarks>
public class RenameFileCommandHandler : IRequestHandler<RenameFileCommand, ErrorOr<FileDto?>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IFileService fileService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="fileService">Injected service for files related functionality.</param>
    public RenameFileCommandHandler(IFileService fileService)
    {
        this.fileService = fileService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Renames a file with the specified name, at the specified path.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a new file result, or an error.</returns>
    public Task<ErrorOr<FileDto?>> Handle(RenameFileCommand command, CancellationToken cancellationToken)
    {
        ErrorOr<File> renameFileResult = fileService.RenameFile(command.Path, command.Name);
        return Task.FromResult(renameFileResult.Match(values => ErrorOrFactory.From(values.Adapt<FileDto?>()), errors => errors));
    }
    #endregion
}