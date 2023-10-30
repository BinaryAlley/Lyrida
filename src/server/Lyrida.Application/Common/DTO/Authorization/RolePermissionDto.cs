#region ========================================================================= USING =====================================================================================
using Mapster;
#endregion

namespace Lyrida.Application.Common.DTO.Authorization;

/// <summary>
/// Role permission data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public sealed class RolePermissionDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = null!;
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Customized ToString() method
    /// </summary>
    /// <returns>Custom string value showing relevant data for current class</returns>
    public override string ToString()
    {
        return Id + " :: " + PermissionName;
    }

    /// <summary>
    /// Maps between this DTO and the coresponding persistance DTO
    /// </summary>
    /// <returns>A data storage DTO representation of this DTO</returns>
    public DataAccess.Common.DTO.Authorization.RolePermissionDto ToStorageDto()
    {
        return this.Adapt<DataAccess.Common.DTO.Authorization.RolePermissionDto>();
    }
    #endregion
}