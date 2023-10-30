#region ========================================================================= USING =====================================================================================
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Attributes;
#endregion

namespace Lyrida.DataAccess.Common.DTO.Configuration;

/// <summary>
/// Profile preferences data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 25th of October, 2023
/// </remarks>
public class ProfilePreferencesDto : IStorageDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    [IgnoreOnCommand]
    [MapsTo(Name = "id")]
    public int Id { get; set; }
    [MapsTo(Name = "user_id")]
    public int UserId { get; set; }
    [MapsTo(Name = "remember_open_tabs")]
    public bool RememberOpenTabs { get; set; } = true;
    [MapsTo(Name = "show_image_previews")]
    public bool ShowImagePreviews { get; set; } = true;
    [MapsTo(Name = "use_2fa")]
    public bool Use2fa { get; set; } = true;
    [MapsTo(Name = "image_previews_quality")]
    public int ImagePreviewsQuality { get; set; }
    [MapsTo(Name = "full_image_quality")]
    public int FullImageQuality { get; set; }
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