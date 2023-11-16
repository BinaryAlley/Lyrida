﻿#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Files.FileProviderStrategies;

/// <summary>
/// Interface for the domain service that defines operations for interacting with directories, providing an abstraction over various directory sources
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
public interface IFileProviderStrategy
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Retrieves a list of files at the specified path.
    /// </summary>
    /// <param name="path">The path for which to retrieve the list of files.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a task representing the asynchronous operation for retrieving a collection of file paths, or an error.</returns>
    ErrorOr<Task<IEnumerable<FileSystemPathId>>> GetFilePathsAsync(FileSystemPathId path);

    /// <summary>
    /// Retrieves the contents of a file at the specified path.
    /// </summary>
    /// <param name="path">The path for which to retrieve the file contents.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a task representing the asynchronous operation for retrieving the contents of a file at the specified path, or an error.</returns>
    ErrorOr<Task<byte[]>> GetFileAsync(FileSystemPathId path);

    /// <summary>
    /// Checks if a file with the specified path exists.
    /// </summary>
    /// <param name="path">The path of the file whose existance is checked.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the result of checking the existance of a file, or an error.</returns>
    ErrorOr<bool> FileExists(FileSystemPathId path);

    /// <summary>
    /// Retrieves the file name from the specified path.
    /// </summary>
    /// <param name="path">The path to extract the file name from.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the name of the file without the path, or the last segment of the path if no file name is found, or an error.</returns>
    ErrorOr<string> GetFileName(FileSystemPathId path);

    /// <summary>
    /// Gets the last write time of a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to retrieve the last write time for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the last write time of <paramref name="path"/>, or null if not available, or an error.</returns>
    ErrorOr<DateTime?> GetLastWriteTime(FileSystemPathId path);

    /// <summary>
    /// Gets the creation time of a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to retrieve the creation time for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the creation time of <paramref name="path"/>, or null if not available, or an error.</returns>
    ErrorOr<DateTime?> GetCreationTime(FileSystemPathId path);

    /// <summary>
    /// Gets the size of a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to retrieve the size for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the size of <paramref name="path"/> or an error.</returns>
    ErrorOr<long?> GetSize(FileSystemPathId path);

    /// <summary>
    /// Copies a file located at <paramref name="sourceFilePath"/> to <paramref name="destinationDirectoryPath"/>.
    /// </summary>
    /// <param name="sourceFilePath">Identifier for the path where the file to be copied is located.</param>
    /// <param name="destinationDirectoryPath">Identifier for the path of the directory where the file will be copied.</param>
    /// <param name="overrideExisting">Whether to override existing files, or not.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the copied file, or an error.</returns>
    ErrorOr<FileSystemPathId> CopyFile(FileSystemPathId sourceFilePath, FileSystemPathId destinationDirectoryPath, bool overrideExisting);

    /// <summary>
    /// Moves a file located at <paramref name="sourceFilePath"/> to <paramref name="destinationDirectoryPath"/>.
    /// </summary>
    /// <param name="sourceFilePath">Identifier for the path where the file to be moved is located.</param>
    /// <param name="destinationDirectoryPath">Identifier for the path of the directory where the file will be moved.</param>
    /// <param name="overrideExisting">Whether to override existing files, or not.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a moved file, or an error.</returns>
    ErrorOr<FileSystemPathId> MoveFile(FileSystemPathId sourceFilePath, FileSystemPathId destinationDirectoryPath, bool overrideExisting);

    /// <summary>
    /// Renames a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to be renamed.</param>
    /// <param name="name">The new name of the file.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the absolute path of the renamed file, or an error.</returns>
    ErrorOr<FileSystemPathId> RenameFile(FileSystemPathId path, string name);

    /// <summary>
    /// Deletes a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to be deleted.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the result of deleting a file, or an error.</returns>
    ErrorOr<bool> DeleteFile(FileSystemPathId path);
    #endregion
}