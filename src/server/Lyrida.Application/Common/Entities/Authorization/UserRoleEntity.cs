#region ========================================================================= USING =====================================================================================
using Mapster;
#endregion

namespace Lyrida.Domain.Common.Entities.Authorization;

/// <summary>
/// User role data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public sealed class UserRoleEntity
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Customized ToString() method
    /// </summary>
    /// <returns>Custom string value showing relevant data for current class</returns>
    public override string ToString()
    {
        return Id + " :: " + RoleName;
    }

    /// <summary>
    /// Maps between this entity and the coresponding persistance entity
    /// </summary>
    /// <returns>A data storage entity representation of this entity</returns>
    public DataAccess.Common.Entities.Authorization.UserRoleEntity ToStorageEntity()
    {
        return this.Adapt<DataAccess.Common.Entities.Authorization.UserRoleEntity>();
    }
    #endregion
}