#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Enums;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
using Lyrida.Domain.Core.FileSystem.Services.Environment;
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
    private readonly IEnvironmentContext environmentContext;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="directoryProvider">Injected service for providing directories</param>
    public DirectoryService(IEnvironmentContextManager environmentContextManager)
    {
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
        ErrorOr<Task<IEnumerable<string>>> subdirectoriesResult = environmentContext.DirectoryProvider.GetSubdirectoryPathsAsync(path.Path);
        if (subdirectoriesResult.IsError)
            return subdirectoriesResult.Errors;
        List<Directory> result = new();
        IEnumerable<string> subdirectories = await subdirectoriesResult.Value;
        foreach (string subPath in subdirectories)
        {
            // extract directory details
            ErrorOr<string> dirNameResult = environmentContext.DirectoryProvider.GetFileName(subPath);
            ErrorOr<DateTime?> dateModifiedResult = environmentContext.DirectoryProvider.GetLastWriteTime(subPath);
            ErrorOr<DateTime?> dateCreatedResult = environmentContext.DirectoryProvider.GetCreationTime(subPath);
            ErrorOr<FileSystemPathId> fileSystemPathIdResult = FileSystemPathId.Create(subPath);

            // If any error occurred, mark directory as Inaccessible
            if (dirNameResult.IsError || dateModifiedResult.IsError || dateCreatedResult.IsError || fileSystemPathIdResult.IsError)
            {
                Directory errorDir = new(!fileSystemPathIdResult.IsError ? fileSystemPathIdResult.Value : null!, !dirNameResult.IsError ? dirNameResult.Value : null!,
                    !dateCreatedResult.IsError ? dateCreatedResult.Value : null, !dateModifiedResult.IsError ? dateModifiedResult.Value : null);
                errorDir.SetStatus(FileSystemItemStatus.Inaccessible);
                result.Add(errorDir);
            }
            else
            {
                Directory subDirectory = new(fileSystemPathIdResult.Value, dirNameResult.Value, dateCreatedResult.Value, dateModifiedResult.Value);
                result.Add(subDirectory);
            }
        }
        return result;
    }
    #endregion
}