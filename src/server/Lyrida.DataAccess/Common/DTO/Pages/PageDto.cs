#region ========================================================================= USING =====================================================================================
using System;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Attributes;
#endregion

namespace Lyrida.DataAccess.Common.DTO.Pages;

/// <summary>
/// User pages data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
public sealed class PageDto : IStorageDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    [IgnoreOnCommand]
    [MapsTo(Name = "id")]
    public int Id { get; set; }
    [MapsTo(Name = "user_id")]
    public int UserId { get; set; }
    [IgnoreOnQuery]
    [MapsTo(Name = "page_id")]
    public Guid Uuid { get; set; }
    [IgnoreOnCommand]
    [MapsTo(Name = "page_id")]
    public string? UuidString { get; set; }
    [MapsTo(Name = "title")]
    public string? Title { get; set; }
    [MapsTo(Name = "path")]
    public string? Path { get; set; }
    [MapsTo(Name = "platform_id")]
    public int PlatformId { get; set; }
    [IgnoreOnCommand]
    [MapsTo(Name = "created")]
    public DateTime Created { get; set; }
    [IgnoreOnCommand]
    [MapsTo(Name = "updated")]
    public DateTime Updated { get; set; }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Customized ToString() method
    /// </summary>
    /// <returns>Custom string value showing relevant data for current class</returns>
    public override string ToString()
    {
        return Id + " :: " + UserId;
    }
    #endregion
}
