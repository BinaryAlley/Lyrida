#region ========================================================================= USING =====================================================================================
using Lyrida.Domain.Common.Enums;
#endregion

namespace Lyrida.Domain.Common.DTO;

/// <summary>
/// Data transfer object for thumbnails
/// </summary>
/// <remarks>
/// Creation Date: 01st of October, 2023
/// </remarks>
public record ThumbnailDto(ImageType Type, byte[] Bytes);