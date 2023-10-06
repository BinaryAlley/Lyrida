﻿#region ========================================================================= USING =====================================================================================
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Attributes;
#endregion

namespace Lyrida.DataAccess.Common.Entities.Authorization;

/// <summary>
/// Permission data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public sealed class PermissionEntity : IStorageEntity
{
    #region ==================================================================== PROPERTIES =================================================================================
    [IgnoreOnCommand]
    [MapsTo(Name = "id")]
    public int Id { get; set; }
    [MapsTo(Name = "permission_name")]
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