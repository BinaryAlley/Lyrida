#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.FileSystem;

/// <summary>
/// Blobs data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 01st of October, 2023
/// </remarks>
[DebuggerDisplay("{ContentType} (Data.Length)")]
public class BlobDataDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public byte[] Data { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    #endregion
}