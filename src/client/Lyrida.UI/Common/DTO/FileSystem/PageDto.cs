#region ========================================================================= USING =====================================================================================
using System;
using System.Diagnostics;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.UI.Common.DTO.FileSystem;

/// <summary>
/// File explorer tab data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 01st of November, 2023
/// </remarks>
[DebuggerDisplay("Path: {Path}")]
public class PageDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public Guid PageId { get; set; }
    public string? Title { get; set; }
    public string? Path { get; set; }
    public Guid? EnvironmentId { get; set; }
    #endregion
}