#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using System.Threading.Tasks;
using System.Collections.Generic;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Directories.DirectoryProviderStrategies;

/// <summary>
/// Interface for the domain service that defines operations for interacting with files, providing an abstraction over underlying storage mechanisms
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
public interface IDirectoryProviderStrategy
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Retrieves a list of subdirectory paths from the specified path, asynchronously.
    /// </summary>
    /// <param name="path">The path from which to retrieve the subdirectory paths.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a task representing the asynchronous operation for retrieving a collection of directory paths or an error.</returns>
    ErrorOr<Task<IEnumerable<string>>> GetSubdirectoryPathsAsync(string path);

    /// <summary>
    /// Retrieves the file name from the specified path.
    /// </summary>
    /// <param name="path">The path to extract the file name from.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a file name or an error.</returns>
    ErrorOr<string> GetFileName(string path);

    /// <summary>
    /// Gets the last write time of a specific path.
    /// </summary>
    /// <param name="path">The path to retrieve the last write time for.</param>
    /// <returns>The last write time for the specified path, or null if unavailable.</returns>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the last write time of <paramref name="path"/>, or null if not available, or an error.</returns>
    ErrorOr<DateTime?> GetLastWriteTime(string path);

    /// <summary>
    /// Gets the creation time of a specific path.
    /// </summary>
    /// <param name="path">The path to retrieve the creation time for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the creation time of <paramref name="path"/>, or null if not available, or an error.</returns>
    ErrorOr<DateTime?> GetCreationTime(string path);
    #endregion
}