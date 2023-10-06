#region ========================================================================= USING =====================================================================================
using ErrorOr;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Enums;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Files.FileTypeStrategies;

/// <summary>
/// Interface defining methods for handling file-related operations based on different sources or platforms
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
public interface IFileTypeStrategy
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Determines if <paramref name="file"/> is of type image or not, and returns its type.
    /// </summary>
    /// <param name="file">The file to determine if it is an image or not</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the type of image or an error.</returns>
    Task<ErrorOr<ImageType>> GetImageTypeAsync(File file);

    /// <summary>
    /// Determines if a file identified by <paramref name="path"/> is of type image or not, and returns its type.
    /// </summary>
    /// <param name="path">The path of the file to determine if it is an image or not</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the type of image or an error.</returns>
    Task<ErrorOr<ImageType>> GetImageTypeAsync(FileSystemPathId path);
    #endregion
}