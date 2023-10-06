#region ========================================================================= USING =====================================================================================
using Lyrida.Domain.Common.Enums;
using System.Collections.Generic;
#endregion

namespace Lyrida.Api.Common.Utilities;

/// <summary>
/// Utility class for converting from domain images types to known image mime types
/// </summary>
/// <remarks>
/// Creation Date: 01st of October, 2023
/// </remarks>
public static class MimeTypes
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private static readonly Dictionary<ImageType, string> ImageTypeToMimeType = new()
    {
        { ImageType.BMP, "image/bmp" },
        { ImageType.GIF, "image/gif" },
        { ImageType.PNG, "image/png" },
        { ImageType.TIFF, "image/tiff" },
        { ImageType.JPEG, "image/jpeg" },
        { ImageType.JPEG_CANON, "image/jpeg" },
        { ImageType.JPEG_UNKNOWN, "image/jpeg" },
        { ImageType.PICT, "image/x-pict" },
        { ImageType.ICO, "image/x-icon" },
        { ImageType.PSD, "image/vnd.adobe.photoshop" },
        { ImageType.JPEG2000, "image/jp2" },
        { ImageType.AVIF, "image/avif" },
        { ImageType.WEBP, "image/webp" },
        { ImageType.TGA, "image/x-tga" },
        { ImageType.SVG, "image/svg+xml" }
    };
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the mime type of <paramref name="type"/>
    /// </summary>
    /// <param name="type">The domain image type for which to get the mime type</param>
    /// <returns>The mime type for <paramref name="type"/></returns>
    public static string GetMimeType(ImageType type)
    {
        return ImageTypeToMimeType.TryGetValue(type, out var mimeType) ? mimeType : "application/octet-stream";
    }
    #endregion
}