#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.FileSystem;

/// <summary>
/// Filesystem element paste data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 14th of November, 2023
/// </remarks>
[DebuggerDisplay("{SourcePath} : {DestinationPath}")]
public class PasteFileSystemItemRequestDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string? FileName { get; set; }
    public string? SourcePath { get; set; }
    public string? DestinationPath { get; set; }
    public bool IsFile { get; set; }
    public bool? OverrideExisting { get; set; }
    #endregion
}