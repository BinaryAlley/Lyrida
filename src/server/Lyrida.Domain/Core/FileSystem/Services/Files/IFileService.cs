#region ========================================================================= USING =====================================================================================
using ErrorOr;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Files;

/// <summary>
/// Interface for the service for handling files
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
public interface IFileService
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Retrieves files for the specified string path.
    /// </summary>
    /// <param name="path">String representation of the file path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of files or an error.</returns>
    Task<ErrorOr<IEnumerable<File>>> GetFilesAsync(string path);

    /// <summary>
    /// Retrieves files associated with a given file.
    /// </summary>
    /// <param name="file">The file object.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of files or an error.</returns>
    Task<ErrorOr<IEnumerable<File>>> GetFilesAsync(File file);

    /// <summary>
    /// Retrieves files for a specified file path ID.
    /// </summary>
    /// <param name="path">Identifier for the file path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of files or an error.</returns>
    Task<ErrorOr<IEnumerable<File>>> GetFilesAsync(FileSystemPathId path);
    #endregion
}