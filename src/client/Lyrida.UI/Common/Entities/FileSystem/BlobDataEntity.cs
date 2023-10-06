#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.Entities.FileSystem;

/// <summary>
/// Blobs data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 01st of October, 2023
/// </remarks>
[DebuggerDisplay("{ContentType} (Data.Length)")]
public record BlobDataEntity(byte[] Data, string ContentType);