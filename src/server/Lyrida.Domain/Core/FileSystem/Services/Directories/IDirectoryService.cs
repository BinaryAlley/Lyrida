#region ========================================================================= USING =====================================================================================
using ErrorOr;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
using Lyrida.Domain.Core.FileSystem.Entities;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Directories;

/// <summary>
/// Interface for the service for handling directories
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
public interface IDirectoryService
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Retrieves subdirectories for the specified string path.
    /// </summary>
    /// <param name="path">String representation of the file path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of subdirectories or an error.</returns>
    Task<ErrorOr<IEnumerable<Directory>>> GetSubdirectoriesAsync(string path);

    /// <summary>
    /// Retrieves subdirectories for the given directory.
    /// </summary>
    /// <param name="directory">Directory object to retrieve subdirectories for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of subdirectories or an error.</returns>
    Task<ErrorOr<IEnumerable<Directory>>> GetSubdirectoriesAsync(Directory directory);

    /// <summary>
    /// Retrieves subdirectories for the specified file system path.
    /// </summary>
    /// <param name="path">Identifier for the file path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of subdirectories or an error.</returns>
    Task<ErrorOr<IEnumerable<Directory>>> GetSubdirectoriesAsync(FileSystemPathId path);
    #endregion
}
