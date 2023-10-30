#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
using System.Collections.Generic;
#endregion

namespace Lyrida.UI.Common.DTO.FileSystem;

/// <summary>
/// Directory data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
[DebuggerDisplay("{Name} (Directory)")]
public class DirectoryDto : FileSystemItemDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public List<FileSystemItemDto> Items { get; set; } = null!;
    #endregion
}