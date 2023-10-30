#region ========================================================================= USING =====================================================================================
using Mapster;
#endregion

namespace Lyrida.Application.Common.DTO.Configuration;

/// <summary>
/// Account preferences data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public sealed class ProfilePreferencesDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool RememberOpenTabs { get; set; } = true;
    public bool ShowImagePreviews { get; set; } = true;
    public bool Use2fa { get; set; } = true;
    public int ImagePreviewsQuality { get; set; }
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

    /// <summary>
    /// Maps between this DTO and the coresponding persistance DTO
    /// </summary>
    /// <returns>A data storage DTO representation of this DTO</returns>
    public DataAccess.Common.DTO.Configuration.ProfilePreferencesDto ToStorageDto()
    {
        return this.Adapt<DataAccess.Common.DTO.Configuration.ProfilePreferencesDto>();
    }
    #endregion
}