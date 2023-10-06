#region ========================================================================= USING =====================================================================================
using ErrorOr;
using System.Threading.Tasks;
using Lyrida.Domain.Common.DTO;
using Lyrida.Domain.Common.Enums;
using Lyrida.Domain.Common.Errors;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
using Lyrida.Domain.Core.FileSystem.Services.Environment;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Thumbnails;

/// <summary>
/// Service that handles thumbnails
/// </summary>
/// <remarks>
/// Creation Date: 28th of September, 2023
/// </remarks>
public class ThumbnailService : IThumbnailService
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IEnvironmentContext environmentContext;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="environmentContextManager">Injected environment context manager used to get the current filesystem environment</param>
    public ThumbnailService(IEnvironmentContextManager environmentContextManager)
    {
        environmentContext = environmentContextManager.GetCurrentContext();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the thumbnail of a file at the specified path.
    /// </summary>
    /// <param name="path">String representation of the file path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of bytes representing the thumbnail of the file at the specified path or an error.</returns>
    public async Task<ErrorOr<ThumbnailDto>> GetThumbnailAsync(string path)
    {
        var fileSystemPathIdResult = FileSystemPathId.Create(path);
        if (fileSystemPathIdResult.IsError)
            return fileSystemPathIdResult.Errors;
        return await GetThumbnailAsync(fileSystemPathIdResult.Value);
    }

    /// <summary>
    /// Gets the thumbnail of a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file for which to get the thumbnail.</param>
    /// <returns></returns>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of bytes representing the thumbnail of the file at the specified path or an error.</returns>
    public async Task<ErrorOr<ThumbnailDto>> GetThumbnailAsync(FileSystemPathId path)
    {
        var imageTypeResult = await environmentContext.FileTypeService.GetImageTypeAsync(path);
        if (imageTypeResult.IsError)
            return imageTypeResult.Errors;
        if (imageTypeResult.Value != ImageType.None)
        {
            var resultFileContents = environmentContext.FileProvider.GetFileAsync(path);
            if (resultFileContents.IsError)
                return resultFileContents.Errors;
            return new ThumbnailDto(imageTypeResult.Value, await resultFileContents.Value);
        }
        else
            return Errors.Thumbnails.NoThumbnail;
    }
    #endregion
}