#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.FileSystem;

/// <summary>
/// Filesystem element rename data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 13th of November, 2023
/// </remarks>
[DebuggerDisplay("{Name}")]
public class RenameFileSystemItemRequestDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string? Path { get; set; }
    public string? Name { get; set; }
    public bool IsFile { get; set; }
    #endregion
}