#region ========================================================================= USING =====================================================================================
using System;
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.FileSystem;

/// <summary>
/// file system data source data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 22nd of November, 2023
/// </remarks>
[DebuggerDisplay("Title: {Title}")]
public class FileSystemDataSourceDto 
{
    #region ==================================================================== PROPERTIES =================================================================================
    public Guid EnvironmentId { get; set; }
    public string? Type { get; set; }
    public string? Title { get; set; }
    public string? InitialPath { get; set; }
    public string? PlatformType { get; set; }
    public string? Url { get; set; }
    public int? Port { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool? PassiveMode { get; set; }
    #endregion
}