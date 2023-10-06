#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using System.Linq;
using System.Text;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Enums;
using Lyrida.Domain.Common.Errors;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
using Lyrida.Domain.Core.FileSystem.Services.Permissions;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Files.FileTypeStrategies;

/// <summary>
/// Class defining methods for handling file-related operations based on local file systems
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
internal class LocalSystemFileTypeStrategy : ILocalSystemFileTypeStrategy
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IFileSystem fileSystem;
    private const int BUFFER_SIZE = 16; // 16 bytes should be more than enough for common images header types
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="fileSystem">Injected service used to interact with the local filesystem</param>
    public LocalSystemFileTypeStrategy(IFileSystem fileSystem)
    {
        this.fileSystem = fileSystem;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Determines if <paramref name="file"/> is of type image or not, and returns its type.
    /// </summary>
    /// <param name="file">The file to determine if it is an image or not</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the type of image or an error.</returns>
    public async Task<ErrorOr<ImageType>> GetImageTypeAsync(File file)
    {
        return await GetImageTypeAsync(file.Id);
    }

    /// <summary>
    /// Determines if a file identified by <paramref name="path"/> is of type image or not, and returns its type.
    /// </summary>
    /// <param name="path">The path of the file to determine if it is an image or not</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the type of image or an error.</returns>
    public async Task<ErrorOr<ImageType>> GetImageTypeAsync(FileSystemPathId path)
    {
        // check if the user has access permissions to the provided path
        if (!FileSystemPermissionsService.CanAccessPath(path.Path, FileAccessMode.ReadContents))
            return Errors.Permission.UnauthorizedAccess;
        Memory<byte> buffer = new byte[BUFFER_SIZE];
        using var stream = fileSystem.FileStream.New(path.Path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
        // check if the file's length is less than the buffer size
        if (stream.Length < BUFFER_SIZE)
            return ImageType.None;
        // read only the first bytes of the file, equal to the buffer size
        await stream.ReadAsync(buffer);
        byte[] firstBytes = buffer[..BUFFER_SIZE].ToArray();
        // check if its a known image type, based on header bytes
        ImageType type = IdentifyHeader(firstBytes);
        if (type != ImageType.None)
            return type;
        // no known image header types were found, check other methods
        string content = Encoding.UTF8.GetString(buffer.ToArray());
        if (IsSvg(content))
            return ImageType.SVG;
        else if (IsTga(buffer.ToArray()))
            return ImageType.TGA;
        else
            return ImageType.None;
    }

    /// <summary>
    /// Determines if the content belongs to an SVG.
    /// </summary>
    private static bool IsSvg(string content)
    {
        // we just check for the SVG tag in the first few lines, though this isn't a comprehensive validation
        return content.Contains("<svg", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Determines if the buffer represents a TGA file.
    /// </summary>
    private static bool IsTga(byte[] buffer)
    {
        if (buffer.Length < 18)
            return false;
        byte imageType = buffer[2];
        return imageType == 1 || imageType == 2 || imageType == 10;
    }

    /// <summary>
    /// Identifies the image type based on the provided header bytes.
    /// </summary>
    /// <param name="firstBytes">The first bytes of the file, typically used to identify the file type.</param>
    /// <returns>The type of the image, or <see cref="{ImageType.None}"/> if unrecognized.</returns>
    private static ImageType IdentifyHeader(byte[] firstBytes)
    {
        if (Encoding.ASCII.GetBytes("BM").SequenceEqual(firstBytes[..2]))
            return ImageType.BMP; // BMP
        if (Encoding.ASCII.GetBytes("GIF").SequenceEqual(firstBytes[..3]))
            return ImageType.GIF; // GIF
        if (new byte[] { 137, 80, 78, 71 }.SequenceEqual(firstBytes[..4]))
            return ImageType.PNG; // PNG
        if (new byte[] { 73, 73, 42 }.SequenceEqual(firstBytes[..3]) || new byte[] { 77, 77, 42 }.SequenceEqual(firstBytes[..3]))
            return ImageType.TIFF; // TIFF
        if (new byte[] { 255, 216, 255, 224 }.SequenceEqual(firstBytes[..4]))
            return ImageType.JPEG; // JPEG
        if (new byte[] { 255, 216, 255, 225 }.SequenceEqual(firstBytes[..4]))
            return ImageType.JPEG_CANON; // JPEG CANON
        if (new byte[] { 255, 216, 255, 226 }.SequenceEqual(firstBytes[..4]))
            return ImageType.JPEG_UNKNOWN; // JPEG UNKNOWN
        if (new byte[] { 0x00, 0x11, 0x02, 0xFF }.SequenceEqual(firstBytes[..4]))
            return ImageType.PICT; // PICT
        if (new byte[] { 0x00, 0x00, 0x01, 0x00 }.SequenceEqual(firstBytes[..4]))
            return ImageType.ICO; // ICO
        if (new byte[] { 0x38, 0x42, 0x50, 0x53 }.SequenceEqual(firstBytes[..4]))
            return ImageType.PSD; // PSD
        if (new byte[] { 0xFF, 0x4F, 0xFF, 0x51 }.SequenceEqual(firstBytes[..4]))
            return ImageType.JPEG2000; // JPEG 2000
        if (new byte[] { 0x00, 0x00, 0x00, 0x0C, 0x6A, 0x70, 0x20, 0x20 }.SequenceEqual(firstBytes[..8]))
            return ImageType.AVIF; // AVIF
        if (new byte[] { 0x00, 0x00, 0x00, 0x0C, 0x6A, 0x50, 0x20, 0x20 }.SequenceEqual(firstBytes[..8]))
            return ImageType.JPEG2000; // JPEG 2000 (variant)
        if (new byte[] { 0x52, 0x49, 0x46, 0x46 }.SequenceEqual(firstBytes[..4]) && new byte[] { 0x57, 0x45, 0x42, 0x50 }.SequenceEqual(firstBytes[8..12]))
            return ImageType.WEBP; // WEBP
        return ImageType.None;
    }
    #endregion
}