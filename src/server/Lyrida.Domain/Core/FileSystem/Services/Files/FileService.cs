#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Common.Enums;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
using Lyrida.Domain.Core.FileSystem.Services.Environment;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Files;

/// <summary>
/// Service for handling files
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
public class FileService : IFileService
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IEnvironmentContext environmentContext;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="environmentContextManager">Injected facade service for environment contextual services</param>
    public FileService(IEnvironmentContextManager environmentContextManager)
    {
        environmentContext = environmentContextManager.GetCurrentContext();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Retrieves files for the specified string path.
    /// </summary>
    /// <param name="path">String representation of the file path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of files or an error.</returns>
    public async Task<ErrorOr<IEnumerable<File>>> GetFilesAsync(string path)
    {
        var fileSystemPathIdResult = FileSystemPathId.Create(path);
        if (fileSystemPathIdResult.IsError)
            return fileSystemPathIdResult.Errors;
        return await GetFilesAsync(fileSystemPathIdResult.Value);
    }

    /// <summary>
    /// Retrieves files associated with a given file.
    /// </summary>
    /// <param name="file">The file object.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of files or an error.</returns>
    public async Task<ErrorOr<IEnumerable<File>>> GetFilesAsync(File file)
    {
        return await GetFilesAsync(file.Id);
    }

    /// <summary>
    /// Retrieves files for a specified file path ID.
    /// </summary>
    /// <param name="path">Identifier for the file path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of files or an error.</returns>
    public async Task<ErrorOr<IEnumerable<File>>> GetFilesAsync(FileSystemPathId path)
    {
        // retrieve the list of files
        ErrorOr<Task<IEnumerable<string>>> filesResult = environmentContext.FileProvider.GetFilePathsAsync(path.Path);
        if (filesResult.IsError)
            return filesResult.Errors;
        List<File> result = new();
        IEnumerable<string> files = await filesResult.Value;
        foreach (string filePath in files)
        {
            // extract file details and add to the result list
            ErrorOr<string> fileNameResult = environmentContext.FileProvider.GetFileName(filePath);
            ErrorOr<DateTime?> dateModifiedResult = environmentContext.FileProvider.GetLastWriteTime(filePath);
            ErrorOr<DateTime?> dateCreatedResult = environmentContext.FileProvider.GetCreationTime(filePath);
            ErrorOr<long?> sizeResult = environmentContext.FileProvider.GetSize(filePath);
            ErrorOr<FileSystemPathId> fileSystemPathIdResult = FileSystemPathId.Create(filePath);
            long size = sizeResult.Value ?? 0;
            // if any of the details returned an error, set inaccessible status
            if (fileNameResult.IsError || dateModifiedResult.IsError || dateCreatedResult.IsError || fileSystemPathIdResult.IsError)
            {
                File errorFile = new(!fileSystemPathIdResult.IsError ? fileSystemPathIdResult.Value : null!, !fileNameResult.IsError ? fileNameResult.Value : null!,
                    !dateCreatedResult.IsError ? dateCreatedResult.Value : null, !dateModifiedResult.IsError ? dateModifiedResult.Value : null, size);
                errorFile.SetStatus(FileSystemItemStatus.Inaccessible);
                result.Add(errorFile);
            }
            else
            {
                File file = new(fileSystemPathIdResult.Value, fileNameResult.Value, dateCreatedResult.Value, dateModifiedResult.Value, size);
                result.Add(file);
            }
        }
        return result;
    }

    public ErrorOr<bool> DeleteFile(FileSystemPathId path)
    {
        return true;
    }

    public ErrorOr<bool> ReadFile(FileSystemPathId path)
    {
        return true;
    }
    #endregion
}