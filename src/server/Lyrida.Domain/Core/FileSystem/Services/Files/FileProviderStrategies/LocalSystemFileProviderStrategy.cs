#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using System.Linq;
using System.IO.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Common.Enums;
using Lyrida.Domain.Common.Errors;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
using Lyrida.Domain.Core.FileSystem.Services.Permissions;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Files.FileProviderStrategies;

/// <summary>
/// File provider for local file systems
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
internal class LocalSystemFileProviderStrategy : ILocalSystemFileProviderStrategy
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
    public LocalSystemFileProviderStrategy(IFileSystem fileSystem, IFileSystemPermissionsService fileSystemPermissionsService)
    {
        this.fileSystem = fileSystem;
        this.fileSystemPermissionsService = fileSystemPermissionsService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Retrieves a list of file paths at the specified path.
    /// </summary>
    /// <param name="path">The path for which to retrieve the list of files.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a task representing the asynchronous operation for retrieving a collection of file paths or an error.</returns>
    public ErrorOr<Task<IEnumerable<FileSystemPathId>>> GetFilePathsAsync(FileSystemPathId path)
    {
        // check if the user has access permissions to the provided path
        if (!fileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ListDirectory))
            return Errors.Permission.UnauthorizedAccess;
        return Task.Run(() => fileSystem.Directory.GetFiles(path.Path)
                                                  .OrderBy(p => p)
                                                  .Select(p => FileSystemPathId.Create(p))
                                                  .Where(errorOrPathId => !errorOrPathId.IsError)
                                                  .Select(errorOrPathId => errorOrPathId.Value)
                                                  .AsEnumerable());
    }

    /// <summary>
    /// Checks if a file with the specified path exists.
    /// </summary>
    /// <param name="path">The path of the file whose existance is checked.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the result of checking the existance of a file, or an error.</returns>
    public ErrorOr<bool> FileExists(FileSystemPathId path)
    {
        return fileSystem.File.Exists(path.Path);
    }

    /// <summary>
    /// Retrieves the file name from the specified path.
    /// </summary>
    /// <param name="path">The path to extract the file name from.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the name of the file without the path, or the last segment of the path if no file name is found, or an error.</returns>
    public ErrorOr<string> GetFileName(FileSystemPathId path)
    {
        return fileSystem.Path.GetFileName(path.Path);
    }

    /// <summary>
    /// Retrieves the contents of a file at the specified path.
    /// </summary>
    /// <param name="path">The path for which to retrieve the file contents.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a task representing the asynchronous operation for retrieving the contents of a file at the specified path, or an error.</returns>
    public ErrorOr<Task<byte[]>> GetFileAsync(FileSystemPathId path)
    {
        // check if the user has access permissions to the provided path
        if (!fileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ReadContents))
            return Errors.Permission.UnauthorizedAccess;
        return Task.Run(() => System.IO.File.ReadAllBytes(path.Path));
    }

    /// <summary>
    /// Gets the last write time of a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to retrieve the last write time for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the last write time of <paramref name="path"/>, or null if not available, or an error.</returns>
    public ErrorOr<DateTime?> GetLastWriteTime(FileSystemPathId path)
    {
        // check if the user has access permissions to the provided path
        if (!fileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ReadProperties))
            return Errors.Permission.UnauthorizedAccess;
        return fileSystem.File.GetLastWriteTime(path.Path);
    }

    /// <summary>
    /// Gets the creation time of a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to retrieve the creation time for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the creation time of <paramref name="path"/>, or null if not available, or an error.</returns>
    public ErrorOr<DateTime?> GetCreationTime(FileSystemPathId path)
    {
        // check if the user has access permissions to the provided path
        if (!fileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ReadProperties))
            return Errors.Permission.UnauthorizedAccess;
        return fileSystem.File.GetCreationTime(path.Path);
    }

    /// <summary>
    /// Gets the size of a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to retrieve the size for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the size of <paramref name="path"/> or an error.</returns>
    public ErrorOr<long?> GetSize(FileSystemPathId path)
    {
        // check if the user has access permissions to the provided path
        if (!fileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ReadProperties))
            return Errors.Permission.UnauthorizedAccess;
        return fileSystem.FileInfo.New(path.Path)?.Length ?? 0;
    }

    /// <summary>
    /// Copies a file located at <paramref name="sourceFilePath"/> to <paramref name="destinationDirectoryPath"/>.
    /// </summary>
    /// <param name="sourceFilePath">Identifier for the path where the file to be copied is located.</param>
    /// <param name="destinationDirectoryPath">Identifier for the path of the directory where the file will be copied.</param>
    /// <param name="overrideExisting">Whether to override existing files, or not.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the copied file, or an error.</returns>
    public ErrorOr<FileSystemPathId> CopyFile(FileSystemPathId sourceFilePath, FileSystemPathId destinationDirectoryPath, bool overrideExisting)
    {
        // check if the source file exists
        if (!fileSystem.File.Exists(sourceFilePath.Path))
            return Errors.FileSystem.FileNotFoundError;
        string destinationFilePath;
        string fileName = fileSystem.Path.GetFileName(sourceFilePath.Path);
        // when copying a file to the same location, just copy it with a new name
        if (fileSystem.Path.GetDirectoryName(sourceFilePath.Path) == destinationDirectoryPath.Path)
            destinationFilePath = CreateUniqueFilePath(destinationDirectoryPath.Path);
        else
        {
            // check if there is already a file with the same name as the copied file, in the destination directory
            if (fileSystem.File.Exists(destinationDirectoryPath.Path + fileName))
                if (!overrideExisting)
                    return Errors.FileSystem.FileAlreadyExistsError;
            destinationFilePath = destinationDirectoryPath.Path + fileName;
        }
        try
        {           
            fileSystem.File.Copy(sourceFilePath.Path, destinationFilePath, overrideExisting); // copy the file
            fileSystem.File.SetAttributes(destinationFilePath, fileSystem.File.GetAttributes(sourceFilePath.Path)); // preserve file attributes
            return FileSystemPathId.Create(destinationFilePath);
        }
        catch
        {
            return Errors.FileSystem.FileCopyError;
        }
    }

    /// <summary>
    /// Creates a file path that is unique
    /// </summary>
    /// <param name="destinationFilePath">The path from which to generate the unique file path.</param>
    /// <returns>A unique directory path.</returns>
    private string CreateUniqueFilePath(string destinationFilePath)
    {
        string? directory = fileSystem.Path.GetDirectoryName(destinationFilePath);
        string? filename = fileSystem.Path.GetFileNameWithoutExtension(destinationFilePath);
        string? extension = fileSystem.Path.GetExtension(destinationFilePath);
        string destFilePath = destinationFilePath;
        int copyNumber = 1;
        // check if the destination file exists and create a unique file name
        if (!string.IsNullOrEmpty(directory) && !string.IsNullOrEmpty(filename) && !string.IsNullOrEmpty(extension))
            while (fileSystem.File.Exists(destFilePath))
                destFilePath = fileSystem.Path.Combine(directory, $"{filename} - Copy ({copyNumber++}){extension}");
        return destFilePath;
    }

    /// <summary>
    /// Moves a file located at <paramref name="sourceFilePath"/> to <paramref name="destinationDirectoryPath"/>.
    /// </summary>
    /// <param name="sourceFilePath">Identifier for the path where the file to be moved is located.</param>
    /// <param name="destinationDirectoryPath">Identifier for the path of the directory where the file will be moved.</param>
    /// <param name="overrideExisting">Whether to override existing files, or not.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a moved file, or an error.</returns>
    public ErrorOr<FileSystemPathId> MoveFile(FileSystemPathId sourceFilePath, FileSystemPathId destinationDirectoryPath, bool overrideExisting)
    {
        // check if the source file exists
        if (!fileSystem.File.Exists(sourceFilePath.Path))
            return Errors.FileSystem.FileNotFoundError;
        try
        {
            string fileName = fileSystem.Path.GetFileName(sourceFilePath.Path);
            string destinationFilePath = destinationDirectoryPath.Path + fileName;
            // if the destination file does not exist, perform a simple move
            if (!fileSystem.File.Exists(destinationFilePath))
                fileSystem.File.Move(sourceFilePath.Path, destinationFilePath);
            else
            {
                if (overrideExisting)
                    fileSystem.File.Move(sourceFilePath.Path, destinationFilePath, overrideExisting);
                else
                    return Errors.FileSystem.FileAlreadyExistsError;
            }
            return FileSystemPathId.Create(destinationFilePath);
        }
        catch 
        {
            return Errors.FileSystem.FileMoveError;
        }
    }

    /// <summary>
    /// Renames a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to be renamed.</param>
    /// <param name="name">The new name of the file.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the absolute path of the renamed file, or an error.</returns>
    public ErrorOr<FileSystemPathId> RenameFile(FileSystemPathId path, string name)
    {
        string? parentDirectory = fileSystem.FileInfo.New(path.Path)?.DirectoryName;
        if (!string.IsNullOrEmpty(parentDirectory))
        {
            ErrorOr<FileSystemPathId> parendDirectoryResult = FileSystemPathId.Create(parentDirectory);
            if (parendDirectoryResult.IsError)
                return parendDirectoryResult.Errors;
            string? newFile = fileSystem.Path.Combine(parendDirectoryResult.Value.Path, name);
            if (!string.IsNullOrEmpty(newFile))
            {
                var newFilePathResult = FileSystemPathId.Create(newFile);
                if (newFilePathResult.IsError)
                    return newFilePathResult.Errors;
                // check if the user has access permissions to the provided path
                if (!fileSystemPermissionsService.CanAccessPath(path, FileAccessMode.Execute))
                    return Errors.Permission.UnauthorizedAccess;
                if (!fileSystemPermissionsService.CanAccessPath(parendDirectoryResult.Value, FileAccessMode.Write))
                    return Errors.Permission.UnauthorizedAccess;
                fileSystem.File.Move(path.Path, newFilePathResult.Value.Path);
                return newFilePathResult;
            }
            else
                return Errors.FileSystem.InvalidPathError;
        }
        else
            return Errors.FileSystem.CannotNavigateUpError;
    }

    /// <summary>
    /// Deletes a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to be deleted.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the result of deleting a file, or an error.</returns>
    public ErrorOr<bool> DeleteFile(FileSystemPathId path)
    {
        // check if the user has access permissions to the provided path
        if (!fileSystemPermissionsService.CanAccessPath(path, FileAccessMode.Delete))
            return Errors.Permission.UnauthorizedAccess;
        fileSystem.File.Delete(path.Path);
        return true;
    }
    #endregion
}