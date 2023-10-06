#region ========================================================================= USING =====================================================================================
using ErrorOr;
using System.Threading.Tasks;
using Lyrida.Domain.Common.DTO;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Thumbnails;

/// <summary>
/// Interface for the service that handles thumbnails
/// </summary>
/// <remarks>
/// Creation Date: 28th of September, 2023
/// </remarks>
public interface IThumbnailService 
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the thumbnail of a file at the specified path.
    /// </summary>
    /// <param name="path">String representation of the file path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of bytes representing the thumbnail of the file at the specified path or an error.</returns>
    Task<ErrorOr<ThumbnailDto>> GetThumbnailAsync(string path);

    /// <summary>
    /// Gets the thumbnail of a file at the specified path.
    /// </summary>
    /// <param name="path">The path object.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of bytes representing the thumbnail of the file at the specified path or an error.</returns>
    Task<ErrorOr<ThumbnailDto>> GetThumbnailAsync(FileSystemPathId path);
    #endregion
}