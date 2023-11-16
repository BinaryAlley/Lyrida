#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Enums;
using System.Collections.Generic;
using Lyrida.Domain.Common.Errors;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
using Lyrida.Domain.Core.FileSystem.Services.Environment;
using Lyrida.Domain.Core.FileSystem.Services.Platform;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Directories;

/// <summary>
/// Service for handling directories
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
public class DirectoryService : IDirectoryService
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IPlatformContext platformContext;
    private readonly IEnvironmentContext environmentContext;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="environmentContextManager">Injected facade service for environment contextual services</param>
    /// <param name="platformContextManager">Injected facade service for platform contextual services</param>
    public DirectoryService(IEnvironmentContextManager environmentContextManager, IPlatformContextManager platformContextManager)
    {
        platformContext = platformContextManager.GetCurrentContext();
        environmentContext = environmentContextManager.GetCurrentContext();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Retrieves subdirectories for the specified string path.
    /// </summary>
    /// <param name="path">String representation of the file path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of subdirectories or an error.</returns>
    public async Task<ErrorOr<IEnumerable<Directory>>> GetSubdirectoriesAsync(string path)
    {
        var fileSystemPathIdResult = FileSystemPathId.Create(path);
        if (fileSystemPathIdResult.IsError)
            return fileSystemPathIdResult.Errors;
        return await GetSubdirectoriesAsync(fileSystemPathIdResult.Value);
    }

    /// <summary>
    /// Retrieves subdirectories for the given directory.
    /// </summary>
    /// <param name="directory">Directory object to retrieve subdirectories for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of subdirectories or an error.</returns>
    public async Task<ErrorOr<IEnumerable<Directory>>> GetSubdirectoriesAsync(Directory directory)
    {
        return await GetSubdirectoriesAsync(directory.Id);
    }

    /// <summary>
    /// Retrieves subdirectories for the specified file system path.
    /// </summary>
    /// <param name="path">Identifier for the file path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of subdirectories or an error.</returns>
    public async Task<ErrorOr<IEnumerable<Directory>>> GetSubdirectoriesAsync(FileSystemPathId path)
    {
        // retrieve the list of subdirectories
        ErrorOr<Task<IEnumerable<FileSystemPathId>>> subdirectoryPathsResult = environmentContext.DirectoryProviderStrategy.GetSubdirectoryPathsAsync(path);
        if (subdirectoryPathsResult.IsError)
            return subdirectoryPathsResult.Errors;
        List<Directory> result = new();
        IEnumerable<FileSystemPathId> subdirectoryPaths = await subdirectoryPathsResult.Value;
        foreach (FileSystemPathId subPath in subdirectoryPaths)
        {
            // extract directory details
            ErrorOr<string> dirNameResult = environmentContext.DirectoryProviderStrategy.GetFileName(subPath);
            ErrorOr<DateTime?> dateModifiedResult = environmentContext.DirectoryProviderStrategy.GetLastWriteTime(subPath);
            ErrorOr<DateTime?> dateCreatedResult = environmentContext.DirectoryProviderStrategy.GetCreationTime(subPath);

            // if any error occurred, mark directory as Inaccessible
            if (dirNameResult.IsError || dateModifiedResult.IsError || dateCreatedResult.IsError)
            {
                Directory errorDir = new(subPath, !dirNameResult.IsError ? dirNameResult.Value : null!,
                    !dateCreatedResult.IsError ? dateCreatedResult.Value : null, !dateModifiedResult.IsError ? dateModifiedResult.Value : null);
                errorDir.SetStatus(FileSystemItemStatus.Inaccessible);
                result.Add(errorDir);
            }
            else
            {
                Directory subDirectory = new(subPath, dirNameResult.Value, dateCreatedResult.Value, dateModifiedResult.Value);
                result.Add(subDirectory);
            }
        }
        return result;
    }

    /// <summary>
    /// Creates a directory with the specified <paramref name="name"/>, at the specified <paramref name="path"/>.
    /// </summary>
    /// <param name="path">String representation of the path where the directory will be created.</param>
    /// <param name="name">The name of the directory that will be created.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the result of creating a directory, or an error.</returns>
    public ErrorOr<Directory> CreateDirectory(string path, string name)
    {
        var fileSystemPathIdResult = FileSystemPathId.Create(path);
        if (fileSystemPathIdResult.IsError)
            return fileSystemPathIdResult.Errors;
        return CreateDirectory(fileSystemPathIdResult.Value, name);
    }

    /// <summary>
    /// Creates a directory with the specified <paramref name="name"/>, at the specified <paramref name="path"/>.
    /// </summary>
    /// <param name="path">Identifier for the path where the directory will be created.</param>
    /// <param name="name">The name of the directory that will be created.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the result of creating a directory, or an error.</returns>
    public ErrorOr<Directory> CreateDirectory(FileSystemPathId path, string name)
    {
        // first, check if the directory about to be created does not already exist
        var combinedPath = platformContext.PathStrategy.CombinePath(path, name);
        if (combinedPath.IsError)
            return combinedPath.Errors;
        var directoryExists = environmentContext.DirectoryProviderStrategy.DirectoryExists(combinedPath.Value);
        if (directoryExists.IsError)
            return directoryExists.Errors;
        else if (directoryExists.Value == true)
            return Errors.FileSystem.DirectoryAlreadyExistsError;
        else
        {
            // create the new directory
            ErrorOr<FileSystemPathId> newDirectoryPathResult = environmentContext.DirectoryProviderStrategy.CreateDirectory(path, name);
            if (newDirectoryPathResult.IsError)
                return newDirectoryPathResult.Errors;
            ErrorOr<string> dirNameResult = environmentContext.DirectoryProviderStrategy.GetFileName(path);
            ErrorOr<DateTime?> dateModifiedResult = environmentContext.DirectoryProviderStrategy.GetLastWriteTime(path);
            ErrorOr<DateTime?> dateCreatedResult = environmentContext.DirectoryProviderStrategy.GetCreationTime(path);
            // if any error occurred, mark directory as Inaccessible
            if (dirNameResult.IsError || dateModifiedResult.IsError || dateCreatedResult.IsError)
            {
                Directory errorDir = new(newDirectoryPathResult.Value, !dirNameResult.IsError ? dirNameResult.Value : null!,
                    !dateCreatedResult.IsError ? dateCreatedResult.Value : null, !dateModifiedResult.IsError ? dateModifiedResult.Value : null);
                errorDir.SetStatus(FileSystemItemStatus.Inaccessible);
                return errorDir;
            }
            else
                return new Directory(newDirectoryPathResult.Value, dirNameResult.Value, dateCreatedResult.Value, dateModifiedResult.Value);
        }
    }

    /// <summary>
    /// Copies a directory located at <paramref name="sourcePath"/> to <paramref name="destinationPath"/>.
    /// </summary>
    /// <param name="sourcePath">String representation of the path where the directory to be copied is located.</param>
    /// <param name="destinationPath">String representation of the path where the directory will be copied.</param>
    /// <param name="overrideExisting">Whether to override existing directories, or not.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a copied directory, or an error.</returns>
    public ErrorOr<Directory> CopyDirectory(string sourcePath, string destinationPath, bool? overrideExisting)
    {
        // make sure the paths are in the expected format
        if (!sourcePath.EndsWith(platformContext.PathStrategy.PathSeparator))
            sourcePath += platformContext.PathStrategy.PathSeparator;
        if (!destinationPath.EndsWith(platformContext.PathStrategy.PathSeparator))
            destinationPath += platformContext.PathStrategy.PathSeparator;
        var fileSystemSourcePathIdResult = FileSystemPathId.Create(sourcePath);
        if (fileSystemSourcePathIdResult.IsError)
            return fileSystemSourcePathIdResult.Errors;
        var fileSystemDestinationPathIdResult = FileSystemPathId.Create(destinationPath);
        if (fileSystemDestinationPathIdResult.IsError)
            return fileSystemDestinationPathIdResult.Errors;
        return CopyDirectory(fileSystemSourcePathIdResult.Value, fileSystemDestinationPathIdResult.Value, overrideExisting ?? false);
    }

    /// <summary>
    /// Copies a directory located at <paramref name="sourcePath"/> to <paramref name="destinationPath"/>.
    /// </summary>
    /// <param name="sourcePath">Identifier for the path where the directory to be copied is located.</param>
    /// <param name="destinationPath">Identifier for the path where the directory will be copied.</param>
    /// <param name="overrideExisting">Whether to override existing directories, or not.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the copied directory, or an error.</returns>
    public ErrorOr<Directory> CopyDirectory(FileSystemPathId sourcePath, FileSystemPathId destinationPath, bool overrideExisting)
    {
        var directoryExists = environmentContext.DirectoryProviderStrategy.DirectoryExists(sourcePath);
        if (directoryExists.IsError)
            return directoryExists.Errors;
        else if (directoryExists.Value == false)
            return Errors.FileSystem.DirectoryNotFoundError;
        else
        {
            // copy the directory
            ErrorOr<FileSystemPathId> newDirectory = environmentContext.DirectoryProviderStrategy.CopyDirectory(sourcePath, destinationPath, overrideExisting);

            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Moves a directory located at <paramref name="sourcePath"/> to <paramref name="destinationPath"/>.
    /// </summary>
    /// <param name="sourcePath">String representation of the path where the directory to be moved is located.</param>
    /// <param name="destinationPath">String representation of the path where the directory will be moved.</param>
    /// <param name="overrideExisting">Whether to override existing directories, or not.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a moved directory, or an error.</returns>
    public ErrorOr<Directory> MoveDirectory(string sourcePath, string destinationPath, bool? overrideExisting)
    {
        // make sure the paths are in the expected format
        if (!sourcePath.EndsWith(platformContext.PathStrategy.PathSeparator))
            sourcePath += platformContext.PathStrategy.PathSeparator;
        if (!destinationPath.EndsWith(platformContext.PathStrategy.PathSeparator))
            destinationPath += platformContext.PathStrategy.PathSeparator;
        var fileSystemSourcePathIdResult = FileSystemPathId.Create(sourcePath);
        if (fileSystemSourcePathIdResult.IsError)
            return fileSystemSourcePathIdResult.Errors;
        var fileSystemDestinationPathIdResult = FileSystemPathId.Create(destinationPath);
        if (fileSystemDestinationPathIdResult.IsError)
            return fileSystemDestinationPathIdResult.Errors;
        return MoveDirectory(fileSystemSourcePathIdResult.Value, fileSystemDestinationPathIdResult.Value, overrideExisting ?? false);
    }

    /// <summary>
    /// Moves a directory for the specified path.
    /// </summary>
    /// <param name="sourcePath">Identifier for the path where the directory to be moved is located.</param>
    /// <param name="destinationPath">Identifier for the path where the directory will be moved.</param>
    /// <param name="overrideExisting">Whether to override existing directories, or not.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the moved directory, or an error.</returns>
    public ErrorOr<Directory> MoveDirectory(FileSystemPathId sourcePath, FileSystemPathId destinationPath, bool overrideExisting)
    {
        var directoryExists = environmentContext.DirectoryProviderStrategy.DirectoryExists(sourcePath);
        if (directoryExists.IsError)
            return directoryExists.Errors;
        else if (directoryExists.Value == false)
            return Errors.FileSystem.DirectoryNotFoundError;
        else
        {
            // move the directory
            ErrorOr<FileSystemPathId> newDirectory = environmentContext.DirectoryProviderStrategy.MoveDirectory(sourcePath, destinationPath, overrideExisting);

            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Renames a directory.
    /// </summary>
    /// <param name="path">String representation of the directory path.</param>
    /// <param name="name">The new name of the directory.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the renamed directory, or an error.</returns>
    public ErrorOr<Directory> RenameDirectory(string path, string name)
    {
        var fileSystemPathIdResult = FileSystemPathId.Create(path);
        if (fileSystemPathIdResult.IsError)
            return fileSystemPathIdResult.Errors;
        return RenameDirectory(fileSystemPathIdResult.Value, name);
    }

    /// <summary>
    /// Renames a directory for the specified path.
    /// </summary>
    /// <param name="path">Identifier for the directory path.</param>
    /// <param name="name">The new name of the directory.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the renamed directory, or an error.</returns>
    public ErrorOr<Directory> RenameDirectory(FileSystemPathId path, string name)
    {
        // first, check if the directory about to be created does not already exist
        var combinedPath = platformContext.PathStrategy.CombinePath(path, name);
        if (combinedPath.IsError)
            return combinedPath.Errors;
        var directoryExists = environmentContext.DirectoryProviderStrategy.DirectoryExists(combinedPath.Value);
        if (directoryExists.IsError)
            return directoryExists.Errors;
        else if (directoryExists.Value == true)
            return Errors.FileSystem.DirectoryAlreadyExistsError;
        else
        {
            // rename the directory
            ErrorOr<FileSystemPathId> newDirectoryPathResult = environmentContext.DirectoryProviderStrategy.RenameDirectory(path, name);
            if (newDirectoryPathResult.IsError)
                return newDirectoryPathResult.Errors;
            ErrorOr<string> dirNameResult = environmentContext.DirectoryProviderStrategy.GetFileName(newDirectoryPathResult.Value);
            ErrorOr<DateTime?> dateModifiedResult = environmentContext.DirectoryProviderStrategy.GetLastWriteTime(newDirectoryPathResult.Value);
            ErrorOr<DateTime?> dateCreatedResult = environmentContext.DirectoryProviderStrategy.GetCreationTime(newDirectoryPathResult.Value);
            // if any error occurred, mark directory as Inaccessible
            if (dirNameResult.IsError || dateModifiedResult.IsError || dateCreatedResult.IsError)
            {
                Directory errorDir = new(newDirectoryPathResult.Value, !dirNameResult.IsError ? dirNameResult.Value : null!,
                    !dateCreatedResult.IsError ? dateCreatedResult.Value : null, !dateModifiedResult.IsError ? dateModifiedResult.Value : null);
                errorDir.SetStatus(FileSystemItemStatus.Inaccessible);
                return errorDir;
            }
            else
                return new Directory(newDirectoryPathResult.Value, dirNameResult.Value, dateCreatedResult.Value, dateModifiedResult.Value);
        }
    }

    /// <summary>
    /// Delete a directory for the specified string path.
    /// </summary>
    /// <param name="path">String representation of the directory path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the result of deleting a directory, or an error.</returns>
    public ErrorOr<bool> DeleteDirectory(string path)
    {
        var fileSystemPathIdResult = FileSystemPathId.Create(path);
        if (fileSystemPathIdResult.IsError)
            return fileSystemPathIdResult.Errors;
        return DeleteDirectory(fileSystemPathIdResult.Value);
    }

    /// <summary>
    /// Delete a directory for the specified path.
    /// </summary>
    /// <param name="path">Identifier for the directory path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the result of deleting a directory, or an error.</returns>
    public ErrorOr<bool> DeleteDirectory(FileSystemPathId path)
    {
        return environmentContext.DirectoryProviderStrategy.DeleteDirectory(path);
    }
    #endregion
}