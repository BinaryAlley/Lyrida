#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
using System.Collections.Generic;
using Lyrida.UI.Common.DTO.FileSystem;
#endregion

namespace Lyrida.UI.Common.DTO.Configuration;

/// <summary>
/// Account preferences data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 25th of October, 2023
/// </remarks>
public class ProfilePreferencesDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public bool RememberOpenTabs { get; set; } = true;
    public bool ShowImagePreviews { get; set; } = true;
    public bool Use2fa { get; set; } = true;
    public int ImagePreviewsQuality { get; set; } = 70;
    public int FullImageQuality { get; set; } = 90;
    public List<PageDto> OpenTabs { get; set; } = new();
    #endregion
}