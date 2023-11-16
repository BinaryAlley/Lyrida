#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.Domain.Core.FileSystem.Services.Files;
#endregion

namespace Lyrida.Application.Core.FileSystem.Files.Commands.Delete;

/// <summary>
/// Delete file command handler
/// </summary>
/// <remarks>
/// Creation Date: 11th of November, 2023
/// </remarks>
public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, ErrorOr<bool>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IFileService fileService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="fileService">Injected service for file related functionality.</param>
    public DeleteFileCommandHandler(IFileService fileService)
    {
        this.fileService = fileService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Deletes a file at the specified path.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a boolean result, or an error.</returns>
    public Task<ErrorOr<bool>> Handle(DeleteFileCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult(fileService.DeleteFile(command.Path));
    }
    #endregion
}