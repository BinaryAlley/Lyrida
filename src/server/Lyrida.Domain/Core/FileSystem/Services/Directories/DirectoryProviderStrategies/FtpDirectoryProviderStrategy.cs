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
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="fileSystem">Injected service used to interact with the local filesystem</param>
    public FtpDirectoryProviderStrategy(IFileSystem fileSystem)
    {
        this.fileSystem = fileSystem;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Retrieves a list of subdirectory paths from the specified path, asynchronously.
    /// </summary>
    /// <param name="path">The path from which to retrieve the subdirectory paths.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a task representing the asynchronous operation for retrieving a collection of directory paths or an error.</returns>
    public ErrorOr<Task<IEnumerable<string>>> GetSubdirectoryPathsAsync(string path)
    {
        // check if the user has access permissions to the provided path
        if (!FileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ListDirectory))
            return Errors.Permission.UnauthorizedAccess;
        return Task.Run(() => fileSystem.Directory.GetDirectories(path).OrderBy(path => path).AsEnumerable());
    }

    /// <summary>
    /// Retrieves the file name from the specified path.
    /// </summary>
    /// <param name="path">The path to extract the file name from.</param>
    /// <returns>The name of the file without the path, or the last segment of the path if no file name is found.</returns>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a file name or an error.</returns>
    public ErrorOr<string> GetFileName(string path)
    {
        return fileSystem.Path.GetFileName(path);
    }

    /// <summary>
    /// Gets the last write time of a specific path.
    /// </summary>
    /// <param name="path">The path to retrieve the last write time for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the last write time of <paramref name="path"/>, or null if not available, or an error.</returns>
    public ErrorOr<DateTime?> GetLastWriteTime(string path)
    {
        // check if the user has access permissions to the provided path
        if (!FileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ReadProperties))
            return Errors.Permission.UnauthorizedAccess;
        return fileSystem.Directory.GetLastWriteTime(path);
    }

    /// <summary>
    /// Gets the creation time of a specific path.
    /// </summary>
    /// <param name="path">The path to retrieve the creation time for.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the creation time of <paramref name="path"/>, or null if not available, or an error.</returns>
    public ErrorOr<DateTime?> GetCreationTime(string path)
    {
        // check if the user has access permissions to the provided path
        if (!FileSystemPermissionsService.CanAccessPath(path, FileAccessMode.ReadProperties))
            return Errors.Permission.UnauthorizedAccess;
        return fileSystem.Directory.GetCreationTime(path);
    }
    #endregion
}