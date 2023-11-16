#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.FileSystem;

/// <summary>
/// Directory create data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 11th of November, 2023
/// </remarks>
[DebuggerDisplay("{Name}")]
public class CreateDirectoryRequestDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string? Path { get; set; }
    public string? Name { get; set; }
    #endregion
}