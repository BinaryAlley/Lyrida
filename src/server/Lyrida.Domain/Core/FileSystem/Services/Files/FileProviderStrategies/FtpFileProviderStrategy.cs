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
/// File provider for FTP services
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
internal class FtpFileProviderStrategy : IFtpFileProviderStrategy
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IFileSystem fileSystem; //  TODO: replace with whatever FTP library
    private readonly IFileSystemPermissionsService fileSystemPermissionsService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="fileSystem">Injected service used to interact with the local filesystem</param>
    /// <param name="fileSystemPermissionsService">Injected service used to determine local filesystem permissions</param>
    public FtpFileProviderStrategy(IFileSystem fileSystem, IFileSystemPermissionsService fileSystemPermissionsService)
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    /// <summary>
    /// Renames a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to be renamed.</param>
    /// <param name="name">The new name of the file.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the absolute path of the renamed file, or an error.</returns>
    public ErrorOr<FileSystemPathId> RenameFile(FileSystemPathId path, string name)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Deletes a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to be deleted.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the result of deleting a file, or an error.</returns>
    public ErrorOr<bool> DeleteFile(FileSystemPathId path)
    {
        throw new NotImplementedException();
    }
    #endregion
}