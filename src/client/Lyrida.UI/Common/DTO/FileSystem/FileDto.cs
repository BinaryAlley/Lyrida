#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.FileSystem;

/// <summary>
/// File data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
[DebuggerDisplay("{Name} (File, Size: {Size} bytes)")]
public class FileDto : FileSystemItemDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public long Size { get; set; }
    #endregion
}