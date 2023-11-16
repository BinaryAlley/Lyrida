﻿#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
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
    ErrorOr<Task<IEnumerable<FileSystemPathId>>> GetSubdirectoryPathsAsync(FileSystemPathId path);

    /// <summary>
    /// Checks if a directory with the specified path exists.
    /// </summary>
    /// <param name="path">The path of the directory whose existance is checked.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the result of checking the existance of a directory, or an error.</returns>
    ErrorOr<bool> DirectoryExists(FileSystemPathId path);

    /// <summary>
    /// Retrieves the file name from the specified path.
    /// </summary>
    /// <param name="path">The path to extract the file name from.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a file name or an error.</returns>
    ErrorOr<string> GetFileName(FileSystemPathId path);

    /// <summary>
    /// Gets the last write time of a specific path.
    /// </summary>
    /// <param name="path">The path to retrieve the last write time for.</param>
    /// <returns>The last write time for the specified path, or null if unavailable.</returns>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the last write time of <paramref name="path"/>, or null if not available, or an error.</returns>
    ErrorOr<DateTime?> GetLastWriteTime(FileSystemPathId path);

    /// <summary>
    /// Gets the creation time of a specific path.
    /// </summary>
    /// <param name="path">The path to retrieve the creation time for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the creation time of <paramref name="path"/>, or null if not available, or an error.</returns>
    ErrorOr<DateTime?> GetCreationTime(FileSystemPathId path);

    /// <summary>
    /// Creates a new directory with the specified name, at the specified path.
    /// </summary>
    /// <param name="path">The path where the directory will be created.</param>
    /// <param name="name">The name of the directory that will be created.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the path of a created directory, or an error.</returns>
    ErrorOr<FileSystemPathId> CreateDirectory(FileSystemPathId path, string name);

    /// <summary>
    /// Copies a directory located at <paramref name="sourcePath"/> to <paramref name="destinationPath"/>.
    /// </summary>
    /// <param name="sourcePath">Identifier for the path where the directory to be copied is located.</param>
    /// <param name="destinationPath">Identifier for the path where the directory will be copied.</param>
    /// <param name="overrideExisting">Whether to override existing directories, or not.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the copied directory, or an error.</returns>
    ErrorOr<FileSystemPathId> CopyDirectory(FileSystemPathId sourcePath, FileSystemPathId destinationPath, bool overrideExisting);

    /// <summary>
    /// Moves a directory located at <paramref name="sourcePath"/> to <paramref name="destinationPath"/>.
    /// </summary>
    /// <param name="sourcePath">Identifier for the path where the directory to be moved is located.</param>
    /// <param name="destinationPath">Identifier for the path where the directory will be moved.</param>
    /// <param name="overrideExisting">Whether to override existing directories, or not.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a moved directory, or an error.</returns>
    ErrorOr<FileSystemPathId> MoveDirectory(FileSystemPathId sourcePath, FileSystemPathId destinationPath, bool overrideExisting);

    /// <summary>
    /// Renames a directory at the specified path.
    /// </summary>
    /// <param name="path">The path of the directory to be renamed.</param>
    /// <param name="name">The new name of the directory.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the absolute path of the renamed directory, or an error.</returns>
    ErrorOr<FileSystemPathId> RenameDirectory(FileSystemPathId path, string name);

    /// <summary>
    /// Deletes a directory at the specified path.
    /// </summary>
    /// <param name="path">The path of the directory to be deleted.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the result of deleting a directory, or an error.</returns>
    ErrorOr<bool> DeleteDirectory(FileSystemPathId path);
    #endregion
}