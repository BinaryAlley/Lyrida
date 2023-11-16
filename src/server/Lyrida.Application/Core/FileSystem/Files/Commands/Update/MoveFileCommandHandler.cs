#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using Mapster;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Application.Common.DTO.FileSystem;
using Lyrida.Domain.Core.FileSystem.Services.Files;
#endregion

namespace Lyrida.Application.Core.FileSystem.Files.Commands.Update;

/// <summary>
/// Move file command handler
/// </summary>
/// <remarks>
/// Creation Date: 14th of November, 2023
/// </remarks>
public class MoveFileCommandHandler : IRequestHandler<MoveFileCommand, ErrorOr<FileDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IFileService directoryService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="fileService">Injected service for files related functionality.</param>
    public MoveFileCommandHandler(IFileService fileService)
    {
        this.directoryService = fileService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Moves a file.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a moved file result, or an error.</returns>
    public Task<ErrorOr<FileDto>> Handle(MoveFileCommand command, CancellationToken cancellationToken)
    {
        ErrorOr<File> renameFileResult = directoryService.MoveFile(command.SourcePath, command.DestinationPath, command.OverrideExisting);
        return Task.FromResult(renameFileResult.Match(values => ErrorOrFactory.From(values.Adapt<FileDto>()), errors => errors));
    }
    #endregion
}