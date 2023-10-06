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
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="fileSystem">Injected service used to interact with the local filesystem</param>
    public FtpFileProviderStrategy(IFileSystem fileSystem)
    {
        this.fileSystem = fileSystem;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Retrieves a list of file paths at the specified path.
    /// </summary>
    /// <param name="path">The path for which to retrieve the list of files.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a task representing the asynchronous operation for retrieving a collection of file paths or an error.</returns>
    public ErrorOr<Task<IEnumerable<string>>> GetFilePathsAsync(string path)
    {
        // check if the user has access permissions to the provided path
        if (!FileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ListDirectory))
            return Errors.Permission.UnauthorizedAccess;
        return Task.Run(() => fileSystem.Directory.GetFiles(path).OrderBy(path => path).AsEnumerable());
    }

    /// <summary>
    /// Retrieves the file name from the specified path.
    /// </summary>
    /// <param name="path">The path to extract the file name from.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the name of the file without the path, or the last segment of the path if no file name is found, or an error.</returns>
    public ErrorOr<string> GetFileName(string path)
    {
        return fileSystem.Path.GetFileName(path);
    }

    /// <summary>
    /// Retrieves the contents of a file at the specified path.
    /// </summary>
    /// <param name="path">The path for which to retrieve the file contents.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a task representing the asynchronous operation for retrieving the contents of a file at the specified path, or an error.</returns>
    public ErrorOr<Task<byte[]>> GetFileAsync(FileSystemPathId path)
    {
        // check if the user has access permissions to the provided path
        if (!FileSystemPermissionsService.CanAccessPath(path.Path, FileAccessMode.ReadContents))
            return Errors.Permission.UnauthorizedAccess;
        return Task.Run(() => System.IO.File.ReadAllBytes(path.Path));
    }

    /// <summary>
    /// Gets the last write time of a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to retrieve the last write time for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the last write time of <paramref name="path"/>, or null if not available, or an error.</returns>
    public ErrorOr<DateTime?> GetLastWriteTime(string path)
    {
        // check if the user has access permissions to the provided path
        if (!FileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ReadProperties))
            return Errors.Permission.UnauthorizedAccess;
        return fileSystem.File.GetLastWriteTime(path);
    }

    /// <summary>
    /// Gets the creation time of a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to retrieve the creation time for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the creation time of <paramref name="path"/>, or null if not available, or an error.</returns>
    public ErrorOr<DateTime?> GetCreationTime(string path)
    {
        // check if the user has access permissions to the provided path
        if (!FileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ReadProperties))
            return Errors.Permission.UnauthorizedAccess;
        return fileSystem.File.GetCreationTime(path);
    }

    /// <summary>
    /// Gets the size of a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to retrieve the size for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the size of <paramref name="path"/> or an error.</returns>
    public ErrorOr<long?> GetSize(string path)
    {
        // check if the user has access permissions to the provided path
        if (!FileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ReadProperties))
            return Errors.Permission.UnauthorizedAccess;
        return fileSystem.FileInfo.New(path)?.Length ?? 0;
    }
    #endregion
}