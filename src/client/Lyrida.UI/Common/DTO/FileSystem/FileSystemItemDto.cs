#region ========================================================================= USING =====================================================================================
using System;
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.FileSystem;

/// <summary>
/// Generic filesystem data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
[DebuggerDisplay("{Name}")]
public class FileSystemItemDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public string Path { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Type { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
    #endregion
}