#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using System.Linq;
using System.IO.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Common.Enums;
using Lyrida.Domain.Common.Errors;
using Lyrida.Domain.Core.FileSystem.Services.Permissions;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Directories.DirectoryProviderStrategies;

/// <summary>
/// Directory provider for FTP services
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
internal class FtpDirectoryProviderStrategy : IFtpDirectoryProviderStrategy
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
    public FtpDirectoryProviderStrategy(IFileSystem fileSystem, IFileSystemPermissionsService fileSystemPermissionsService)
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    /// <summary>
    /// Renames a directory at the specified path.
    /// </summary>
    /// <param name="path">The path of the directory to be renamed.</param>
    /// <param name="name">The new name of the directory.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the absolute path of the renamed directory, or an error.</returns>
    public ErrorOr<FileSystemPathId> RenameDirectory(FileSystemPathId path, string name)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Deletes a directory at the specified path.
    /// </summary>
    /// <param name="path">The path of the directory to be deleted.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the result of deleting a directory, or an error.</returns>
    public ErrorOr<bool> DeleteDirectory(FileSystemPathId path)
    {
        throw new NotImplementedException();
    }
    #endregion
}