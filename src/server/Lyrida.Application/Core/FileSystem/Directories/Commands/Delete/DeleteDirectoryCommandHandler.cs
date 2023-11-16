#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.Domain.Core.FileSystem.Services.Directories;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Commands.Delete;

/// <summary>
/// Delete directory command handler
/// </summary>
/// <remarks>
/// Creation Date: 11th of November, 2023
/// </remarks>
public class DeleteDirectoryCommandHandler : IRequestHandler<DeleteDirectoryCommand, ErrorOr<bool>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDirectoryService directoryService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="fileService">Injected service for directory related functionality.</param>
    public DeleteDirectoryCommandHandler(IDirectoryService directoryService)
    {
        this.directoryService = directoryService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Deletes a directory at the specified path.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a boolean result, or an error.</returns>
    public Task<ErrorOr<bool>> Handle(DeleteDirectoryCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult(directoryService.DeleteDirectory(command.Path));
    }
    #endregion
}