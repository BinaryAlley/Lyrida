#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using System.IO;
using SixLabors.ImageSharp;
using System.Threading.Tasks;
using Lyrida.Domain.Common.DTO;
using Lyrida.Domain.Common.Enums;
using Lyrida.Domain.Common.Errors;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
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
    /// <param name="quality">The quality of the thumbnail to get.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of bytes representing the thumbnail of the file at the specified path or an error.</returns>
    public async Task<ErrorOr<ThumbnailDto>> GetThumbnailAsync(string path, int quality)
    {
        var fileSystemPathIdResult = FileSystemPathId.Create(path);
        if (fileSystemPathIdResult.IsError)
            return fileSystemPathIdResult.Errors;
        return await GetThumbnailAsync(fileSystemPathIdResult.Value, quality);
    }

    /// <summary>
    /// Gets the thumbnail of a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file for which to get the thumbnail.</param>
    /// <param name="quality">The quality of the thumbnail to get.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of bytes representing the thumbnail of the file at the specified path or an error.</returns>
    public async Task<ErrorOr<ThumbnailDto>> GetThumbnailAsync(FileSystemPathId path, int quality)
    {
        // first, get the type of the image file
        var imageTypeResult = await environmentContext.FileTypeStrategy.GetImageTypeAsync(path);
        if (imageTypeResult.IsError)
            return imageTypeResult.Errors;
        if (imageTypeResult.Value != ImageType.None)
        {
            // then, get its bytes
            var resultFileContents = environmentContext.FileProviderStrategy.GetFileAsync(path);
            if (resultFileContents.IsError)
                return resultFileContents.Errors;
            byte[] fileContents = await resultFileContents.Value;
            // finally, resize or adjust quality based on image type
            byte[]? adjustedImage = null;
            if (imageTypeResult.Value == ImageType.JPEG || imageTypeResult.Value == ImageType.JPEG_CANON || imageTypeResult.Value == ImageType.JPEG2000 
                || imageTypeResult.Value == ImageType.JPEG_UNKNOWN || imageTypeResult.Value == ImageType.PNG || imageTypeResult.Value == ImageType.BMP
                || imageTypeResult.Value == ImageType.WEBP || imageTypeResult.Value == ImageType.GIF || imageTypeResult.Value == ImageType.TIFF || imageTypeResult.Value == ImageType.TGA)
                adjustedImage = await AdjustImageResolutionAsync(fileContents, imageTypeResult.Value, quality);
            return new ThumbnailDto(imageTypeResult.Value, adjustedImage ?? fileContents);
        }
        else
            return Errors.Thumbnails.NoThumbnail;
    }

    /// <summary>
    /// Adjusts the quality or compression of an image, based on its format.
    /// </summary>
    /// <param name="imageBytes">The byte array of the source image.</param>
    /// <param name="imageType">The format of the image whose quality is adjusted.</param>
    /// <param name="quality">A value between 1 (lowest quality) to 100 (highest quality).</param>
    /// <returns>A byte array representing the processed image.</returns>
    private static async Task<byte[]> AdjustImageQualityAsync(byte[] imageBytes, ImageType imageType, int quality)
    {
        using var inputMemoryStream = new MemoryStream(imageBytes);
        using var outputMemoryStream = new MemoryStream();       
        switch (imageType)
        {
            case ImageType.JPEG:
            case ImageType.JPEG2000:
            case ImageType.JPEG_CANON:
            case ImageType.JPEG_UNKNOWN:
                {
                    using var image = await Image.LoadAsync(inputMemoryStream);
                    await image.SaveAsync(outputMemoryStream, new JpegEncoder() { Quality = quality });
                    break;
                }
            case ImageType.PNG:
                {
                    using var image = await Image.LoadAsync(inputMemoryStream);
                    // PNG is lossless; quality adjustments don't apply in the same way - we can adjust compression
                    int compressionLevel = MapRange(quality, 1, 100, 1, 9);
                    var encoder = new PngEncoder { CompressionLevel = (PngCompressionLevel)compressionLevel };
                    await image.SaveAsync(outputMemoryStream, encoder);
                    break;
                }
            case ImageType.BMP:
                {
                    using var image = await Image.LoadAsync(inputMemoryStream);
                    await image.SaveAsync(outputMemoryStream, new PngEncoder()); // convert BMP to PNG for compression
                    break;
                }
            default:
                return imageBytes; // for unsupported formats, just return the original bytes
        }
        return outputMemoryStream.ToArray();
    }

    /// <summary>
    /// Adjusts the quality or size of an image, based on its format.
    /// </summary>
    /// <param name="imageBytes">The byte array of the source image.</param>
    /// <param name="imageType">The format of the image whose quality is adjusted.</param>
    /// <param name="quality">A value between 1 (lowest quality) to 100 (highest quality).</param>
    /// <returns></returns>
    private static async Task<byte[]> AdjustImageResolutionAsync(byte[] imageBytes, ImageType imageType, int quality)
    {
        using var inputMemoryStream = new MemoryStream(imageBytes);
        using var outputMemoryStream = new MemoryStream();
        Image image = await Image.LoadAsync(inputMemoryStream);
        double ratio = quality / 100.0;
        int newWidth = (int)(image.Width * ratio);
        int newHeight = (int)(image.Height * ratio);
        // ensure new dimensions are not zero
        newWidth = Math.Max(1, newWidth);
        newHeight = Math.Max(1, newHeight);
        var options = new ResizeOptions
        {
            Size = new Size(newWidth, newHeight),
            Mode = ResizeMode.Max
        };
        // resize the image to the specified size
        image.Mutate(ctx => ctx.Resize(options));
        // save the resized image and return its bytes
        switch (imageType)
        {
            case ImageType.BMP:
            case ImageType.PNG:
            case ImageType.GIF:
            case ImageType.TGA:
            case ImageType.WEBP:
            case ImageType.TIFF:
            case ImageType.JPEG:
            case ImageType.JPEG2000:
            case ImageType.JPEG_CANON:
            case ImageType.JPEG_UNKNOWN:
                await image.SaveAsync(outputMemoryStream, image.Metadata.DecodedImageFormat!);
                break;
            default:
                return imageBytes; // for unsupported formats, just return the original bytes
        }       
        return outputMemoryStream.ToArray();
    }

    /// <summary>
    /// Maps a given value from a source range to a target range.
    /// </summary>
    /// <param name="value">The value to map.</param>
    /// <param name="fromSource">The inclusive lower bound of the source range.</param>
    /// <param name="toSource">The inclusive upper bound of the source range.</param>
    /// <param name="fromTarget">The inclusive lower bound of the target range.</param>
    /// <param name="toTarget">The inclusive upper bound of the target range.</param>
    /// <returns>
    /// A value mapped to the target range.
    /// </returns>
    private static int MapRange(int value, int fromSource, int toSource, int fromTarget, int toTarget)
    {
        return (value - fromSource) * (toTarget - fromTarget) / (toSource - fromSource) + fromTarget;
    }
    #endregion
}