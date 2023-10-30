#region ========================================================================= USING =====================================================================================
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Attributes;
#endregion

namespace Lyrida.DataAccess.Common.DTO.Authorization;

/// <summary>
/// Role permission data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public sealed class RolePermissionDto : IStorageDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    [IgnoreOnCommand]
    [MapsTo(Name = "id")]
    public int Id { get; set; }
    [MapsTo(Name = "role_id")]
    public int RoleId { get; set; }
    [MapsTo(Name = "permission_id")]
    public int PermissionId { get; set; }
    [IgnoreOnQuery]
    [IgnoreOnCommand]
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
    #endregion
}