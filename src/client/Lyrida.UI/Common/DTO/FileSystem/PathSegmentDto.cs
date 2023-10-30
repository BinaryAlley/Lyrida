#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.FileSystem;

/// <summary>
/// Path segment data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 04th of October, 2023
/// </remarks>
[DebuggerDisplay("{Name} (IsDirectory: {IsDirectory}, IsDrive: {IsDrive})")]
public class PathSegmentDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string Name { get; set; } = null!;
    public bool IsDirectory { get; set; }
    public bool IsDrive { get; set; }
    #endregion
}