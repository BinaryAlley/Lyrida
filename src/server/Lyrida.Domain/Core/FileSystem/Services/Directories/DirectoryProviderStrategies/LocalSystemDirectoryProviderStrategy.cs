#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Abstractions;
using System.Collections.Generic;
using Lyrida.Domain.Common.Enums;
using Lyrida.Domain.Common.Errors;
using Lyrida.Domain.Core.FileSystem.Services.Permissions;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Directories.DirectoryProviderStrategies;

/// <summary>
/// Directory provider for local file systems
/// </summary>
/// <remarks>
/// Creation Date: 22nd of September, 2023
/// </remarks>
internal class LocalSystemDirectoryProviderStrategy : ILocalSystemDirectoryProviderStrategy
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IFileSystem fileSystem;
    private readonly IFileSystemPermissionsService fileSystemPermissionsService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="fileSystem">Injected service used to interact with the local filesystem</param>
    /// <param name="fileSystemPermissionsService">Injected service used to determine local filesystem permissions</param>
    public LocalSystemDirectoryProviderStrategy(IFileSystem fileSystem, IFileSystemPermissionsService fileSystemPermissionsService)
    {
        this.fileSystem = fileSystem;
        this.fileSystemPermissionsService = fileSystemPermissionsService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Retrieves a list of subdirectory paths from the specified path, asynchronously.
    /// </summary>
    /// <param name="path">The path from which to retrieve the subdirectory paths.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a task representing the asynchronous operation for retrieving a collection of directory paths or an error.</returns>
    public ErrorOr<Task<IEnumerable<FileSystemPathId>>> GetSubdirectoryPathsAsync(FileSystemPathId path)
    {
        // check if the user has access permissions to the provided path
        if (!fileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ListDirectory, false))
            return Errors.Permission.UnauthorizedAccess;
        return Task.Run(() => fileSystem.Directory.GetDirectories(path.Path)
                                                  .OrderBy(p => p)
                                                  .Select(p => FileSystemPathId.Create(p))
                                                  .Where(errorOrPathId => !errorOrPathId.IsError)
                                                  .Select(errorOrPathId => errorOrPathId.Value)
                                                  .AsEnumerable());
    }

    /// <summary>
    /// Checks if a directory with the specified path exists.
    /// </summary>
    /// <param name="path">The path of the directory whose existance is checked.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the result of checking the existance of a directory, or an error.</returns>
    public ErrorOr<bool> DirectoryExists(FileSystemPathId path)
    {
        return fileSystem.Directory.Exists(path.Path);
    }

    /// <summary>
    /// Retrieves the file name from the specified path.
    /// </summary>
    /// <param name="path">The path to extract the file name from.</param>
    /// <returns>The name of the file without the path, or the last segment of the path if no file name is found.</returns>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a file name or an error.</returns>
    public ErrorOr<string> GetFileName(FileSystemPathId path)
    {
        return fileSystem.Path.GetFileName(path.Path);
    }

    /// <summary>
    /// Gets the last write time of a specific path.
    /// </summary>
    /// <param name="path">The path to retrieve the last write time for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the last write time of <paramref name="path"/>, or null if not available, or an error.</returns>
    public ErrorOr<DateTime?> GetLastWriteTime(FileSystemPathId path)
    {
        // check if the user has access permissions to the provided path
        if (!fileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ReadProperties, false))
            return Errors.Permission.UnauthorizedAccess;
        return fileSystem.Directory.GetLastWriteTime(path.Path);
    }

    /// <summary>
    /// Gets the creation time of a specific path.
    /// </summary>
    /// <param name="path">The path to retrieve the creation time for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the creation time of <paramref name="path"/>, or null if not available, or an error.</returns>
    public ErrorOr<DateTime?> GetCreationTime(FileSystemPathId path)
    {
        // check if the user has access permissions to the provided path
        if (!fileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ReadProperties, false))
            return Errors.Permission.UnauthorizedAccess;
        return fileSystem.Directory.GetCreationTime(path.Path);
    }

    /// <summary>
    /// Creates a new directory with the specified name, at the specified path.
    /// </summary>
    /// <param name="path">The path where the directory will be created..</param>
    /// <param name="name">The name of the directory that will be created.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the path of a created directory, or an error.</returns>
    public ErrorOr<FileSystemPathId> CreateDirectory(FileSystemPathId path, string name)
    {
        // to create a directory, its parent directory must be writable
        if (!fileSystemPermissionsService.CanAccessPath(path, FileAccessMode.Write, false))
            return Errors.Permission.UnauthorizedAccess;
        // create the directory and return its absolute path
        string directoryPath = fileSystem.Directory.CreateDirectory(fileSystem.Path.Combine(path.Path, name)).FullName;
        return FileSystemPathId.Create(directoryPath);
    }

    /// <summary>
    /// Copies a directory for the specified path.
    /// </summary>
    /// <param name="sourcePath">Identifier for the path where the directory to be copied is located.</param>
    /// <param name="destinationPath">Identifier for the path where the directory will be copied.</param>
    /// <param name="overrideExisting">Whether to override existing directories, or not.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the copied directory, or an error.</returns>
    public ErrorOr<FileSystemPathId> CopyDirectory(FileSystemPathId sourcePath, FileSystemPathId destinationPath, bool overrideExisting)
    {
        // check if the source directory exists
        if (!fileSystem.Directory.Exists(sourcePath.Path))
            return Errors.FileSystem.DirectoryNotFoundError;
        string destPath = CreateUniqueDirectoryPath(destinationPath.Path);
        try
        {
            // create the new directory
            IDirectoryInfo destDirInfo = fileSystem.DirectoryInfo.New(fileSystem.Directory.CreateDirectory(destPath).FullName);
            // get the information of the source directory
            IDirectoryInfo sourceDirInfo = fileSystem.DirectoryInfo.New(sourcePath.Path);
            // copy all files
            foreach (IFileInfo fileInfo in sourceDirInfo.GetFiles())
            {
                string targetFilePath = fileSystem.Path.Combine(destPath, fileInfo.Name);
                fileInfo.CopyTo(targetFilePath, overrideExisting);
                fileSystem.File.SetAttributes(targetFilePath, fileInfo.Attributes); // preserve file attributes
            }
            // copy all subdirectories
            foreach (IDirectoryInfo subDirInfo in sourceDirInfo.GetDirectories())
            {
                ErrorOr<FileSystemPathId> newSourcePathResult = FileSystemPathId.Create(subDirInfo.FullName);
                if (newSourcePathResult.IsError)
                    return newSourcePathResult.Errors;
                ErrorOr<FileSystemPathId> newDestinationPathResult = FileSystemPathId.Create(fileSystem.Path.Combine(destPath, subDirInfo.Name));
                if (newDestinationPathResult.IsError)
                    return newDestinationPathResult.Errors;
                // recursive call
                ErrorOr<FileSystemPathId> copySubDirResult = CopyDirectory(newSourcePathResult.Value, newDestinationPathResult.Value, overrideExisting);
                if (copySubDirResult.IsError)
                    return copySubDirResult.Errors;
            }
            return FileSystemPathId.Create(destPath);
        }
        catch
        {
            return Errors.FileSystem.DirectoryCopyError;
        }
    }

    /// <summary>
    /// Creates a directory path that is unique.
    /// </summary>
    /// <param name="destinationPath">The path from which to generate the unique directory path.</param>
    /// <returns>A unique directory path.</returns>
    private string CreateUniqueDirectoryPath(string destinationPath)
    {
        string destPath = destinationPath;
        int copyNumber = 1;
        // check if the destination directory exists and create a unique directory name
        while (fileSystem.Directory.Exists(destPath) || fileSystem.File.Exists(destPath))
            destPath = $"{destinationPath} - Copy ({copyNumber++})";
        return destPath;
    }

    /// <summary>
    /// Moves a directory located at <paramref name="sourcePath"/> to <paramref name="destinationPath"/>.
    /// </summary>
    /// <param name="sourcePath">Identifier for the path where the directory to be moved is located.</param>
    /// <param name="destinationPath">Identifier for the path where the directory will be moved.</param>
    /// <param name="overrideExisting">Whether to override existing directories, or not.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a moved directory, or an error.</returns>
    public ErrorOr<FileSystemPathId> MoveDirectory(FileSystemPathId sourcePath, FileSystemPathId destinationPath, bool overrideExisting)
    {
        // check if the source directory exists
        if (!fileSystem.Directory.Exists(sourcePath.Path))
            return Errors.FileSystem.DirectoryNotFoundError;
        try
        {
            // if the destination directory does not exist, perform a simple move
            if (!fileSystem.Directory.Exists(destinationPath.Path))
                fileSystem.Directory.Move(sourcePath.Path, destinationPath.Path);
            else
                MergeDirectories(sourcePath.Path, destinationPath.Path, overrideExisting); // if the destination directory exists, perform a merge based on the overwrite policy
            return FileSystemPathId.Create(destinationPath.Path);
        }
        catch
        {
            return Errors.FileSystem.DirectoryMoveError;
        }
    }

    /// <summary>
    /// Merges two directories.
    /// </summary>
    /// <param name="sourceDir">The source directory to be merged.</param>
    /// <param name="destDir">The destination directory to be merged.</param>
    /// <param name="overwrite">Whether to override existing files.</param>
    private void MergeDirectories(string sourceDir, string destDir, bool overwrite)
    {
        // merge files from source directory to destination directory
        foreach (string sourceFilePath in fileSystem.Directory.GetFiles(sourceDir))
        {
            string fileName = fileSystem.Path.GetFileName(sourceFilePath);
            string destFilePath = fileSystem.Path.Combine(destDir, fileName);
            // if a file with the same name exists in the destination
            if (fileSystem.File.Exists(destFilePath))
            {
                if (overwrite)
                {
                    // overwrite the file in the destination directory
                    fileSystem.File.Delete(destFilePath);
                    fileSystem.File.Move(sourceFilePath, destFilePath);
                }
                // otherwise, skip the file
            }
            else
                fileSystem.File.Move(sourceFilePath, destFilePath); // move the file if it does not exist in the destination directory
        }
        // recursively merge subdirectories
        foreach (string sourceSubDirPath in fileSystem.Directory.GetDirectories(sourceDir))
        {
            string subDirName = fileSystem.Path.GetFileName(sourceSubDirPath);
            string destSubDirPath = fileSystem.Path.Combine(destDir, subDirName);
            // if the subdirectory does not exist in the destination, move it
            if (!fileSystem.Directory.Exists(destSubDirPath))
                fileSystem.Directory.Move(sourceSubDirPath, destSubDirPath);
            else
                MergeDirectories(sourceSubDirPath, destSubDirPath, overwrite); // if the subdirectory exists, recursively merge its contents
        }
        // after merging the contents, delete the source directory if it's now empty
        if (!fileSystem.Directory.EnumerateFileSystemEntries(sourceDir).Any())
            fileSystem.Directory.Delete(sourceDir);
    }

    /// <summary>
    /// Renames a directory at the specified path.
    /// </summary>
    /// <param name="path">The path of the directory to be renamed.</param>
    /// <param name="name">The new name of the directory.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the absolute path of the renamed directory, or an error.</returns>
    public ErrorOr<FileSystemPathId> RenameDirectory(FileSystemPathId path, string name)
    {
        // to rename a directory, its parent directory must be writable
        string? parentDirectory = fileSystem.Directory.GetParent(path.Path)?.FullName;
        if (!string.IsNullOrEmpty(parentDirectory))
        {
            ErrorOr<FileSystemPathId> parendDirectoryResult = FileSystemPathId.Create(parentDirectory);
            if (parendDirectoryResult.IsError)
                return parendDirectoryResult.Errors;
            string? newDirectory = fileSystem.Path.Combine(parentDirectory, name);
            if (!string.IsNullOrEmpty(newDirectory))
            {
                var newDirectoryPathResult = FileSystemPathId.Create(newDirectory);
                if (newDirectoryPathResult.IsError)
                    return newDirectoryPathResult.Errors;
                if (!fileSystemPermissionsService.CanAccessPath(parendDirectoryResult.Value, FileAccessMode.Write, false))
                    return Errors.Permission.UnauthorizedAccess;
                // to rename a directory, it must be executable
                if (!fileSystemPermissionsService.CanAccessPath(path, FileAccessMode.Execute, false))
                    return Errors.Permission.UnauthorizedAccess;
                fileSystem.Directory.Move(path.Path, newDirectoryPathResult.Value.Path);
                return newDirectoryPathResult.Value;
            }
            else
                return Errors.FileSystem.InvalidPathError;
        }
        else
            return Errors.FileSystem.InvalidPathError;
    }

    /// <summary>
    /// Deletes a directory at the specified path.
    /// </summary>
    /// <param name="path">The path of the directory to be deleted.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the result of deleting a directory, or an error.</returns>
    public ErrorOr<bool> DeleteDirectory(FileSystemPathId path)
    {
        // check if the user has access permissions to the provided path
        if (!fileSystemPermissionsService.CanAccessPath(path, FileAccessMode.Delete, false))
            return Errors.Permission.UnauthorizedAccess;
        fileSystem.DirectoryInfo.New(path.Path).Delete(true);
        return true;
    }
    #endregion
}