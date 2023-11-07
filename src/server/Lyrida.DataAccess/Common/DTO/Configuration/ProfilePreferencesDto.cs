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
public sealed class ProfilePreferencesDto : IStorageDto
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
    [MapsTo(Name = "inspect_file_for_thumbnails")]
    public bool InspectFileForThumbnails { get; set; } = true;
    [MapsTo(Name = "enable_console_debug_messages")]
    public bool EnableConsoleDebugMessages { get; set; } = false;
    [MapsTo(Name = "scroll_thumbnail_retrieval_timeout")]
    public int ScrollThumbnailRetrievalTimeout { get; set; }
    [MapsTo(Name = "thumbnails_retrieval_batch_size")]
    public int ThumbnailsRetrievalBatchSize { get; set; }
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