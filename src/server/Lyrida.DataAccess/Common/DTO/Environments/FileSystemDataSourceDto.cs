#region ========================================================================= USING =====================================================================================
using System;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Attributes;
#endregion

namespace Lyrida.DataAccess.Common.DTO.Environments;

/// <summary>
/// User environments data transfer object.
/// </summary>
/// <remarks>
/// Creation Date: 22nd of November, 2023
/// </remarks>
public sealed class FileSystemDataSourceDto : IStorageDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    [IgnoreOnCommand]
    [MapsTo(Name = "id")]
    public int Id { get; set; }
    [MapsTo(Name = "user_id")]
    public int UserId { get; set; }
    [IgnoreOnQuery]
    [MapsTo(Name = "environment_id")]
    public Guid EnvironmentId { get; set; }
    [IgnoreOnCommand]
    [MapsTo(Name = "environment_id")]
    public string EnvironmentIdString { get; set; } = null!;
    [MapsTo(Name = "title")]
    public string Title { get; set; } = null!;
    [MapsTo(Name = "type")]
    public string Type { get; set; } = null!;
    [MapsTo(Name = "initial_path")]
    public string InitialPath { get; set; } = null!;
    [MapsTo(Name = "platform_type")]
    public string PlatformType { get; set; } = null!;
    [MapsTo(Name = "url")]
    public string? Url { get; set; }
    [MapsTo(Name = "port")]
    public int? Port { get; set; }
    [MapsTo(Name = "username")]
    public string? Username { get; set; }
    [MapsTo(Name = "password")]
    public string? Password { get; set; }
    [MapsTo(Name = "passive_mode")]
    public bool? PassiveMode { get; set; }
    [IgnoreOnCommand]
    [MapsTo(Name = "created")]
    public DateTime Created { get; set; }
    [IgnoreOnCommand]
    [MapsTo(Name = "updated")]
    public DateTime Updated { get; set; }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Customized ToString() method.
    /// </summary>
    /// <returns>Custom string value showing relevant data for current class.</returns>
    public override string ToString()
    {
        return Id + " :: " + UserId;
    }
    #endregion
}