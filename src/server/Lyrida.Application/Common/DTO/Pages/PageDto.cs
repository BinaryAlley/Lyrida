#region ========================================================================= USING =====================================================================================
using System;
using Mapster;
#endregion

namespace Lyrida.Application.Common.DTO.Pages;

/// <summary>
/// User pages data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
public sealed class PageDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public int UserId { get; set; }
    public Guid Uuid { get; set; }
    public string? Title { get; set; }
    public string? Path { get; set; }
    public int PlatformId { get; set; }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Customized ToString() method
    /// </summary>
    /// <returns>Custom string value showing relevant data for current class</returns>
    public override string ToString()
    {
        return Id + " :: " + Path;
    }

    /// <summary>
    /// Maps between this DTO and the coresponding persistance DTO
    /// </summary>
    /// <returns>A data storage DTO representation of this DTO</returns>
    public DataAccess.Common.DTO.Pages.PageDto ToStorageDto()
    {
        return this.Adapt<DataAccess.Common.DTO.Pages.PageDto>();
    }
    #endregion
}