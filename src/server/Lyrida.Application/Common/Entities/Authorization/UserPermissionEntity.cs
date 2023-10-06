#region ========================================================================= USING =====================================================================================
using Mapster;
#endregion

namespace Lyrida.Domain.Common.Entities.Authorization;

/// <summary>
/// User permission data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public sealed class UserPermissionEntity
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public int UserId { get; set; }
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
    /// Maps between this entity and the coresponding persistance entity
    /// </summary>
    /// <returns>A data storage entity representation of this entity</returns>
    public DataAccess.Common.Entities.Authorization.RolePermissionEntity ToStorageEntity()
    {
        return this.Adapt<DataAccess.Common.Entities.Authorization.RolePermissionEntity>();
    }
    #endregion
}