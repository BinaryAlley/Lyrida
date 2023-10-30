#region ========================================================================= USING =====================================================================================
using System.Linq;
using System.Collections.Generic;
using Lyrida.Application.Common.DTO.Authorization;
#endregion

namespace Lyrida.Application.Core.Authorization;

/// <summary>
/// Class for defining permissions of a user
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public class UserPermissions
{
    #region ==================================================================== PROPERTIES =================================================================================
    public bool CanViewUsers { get; private set; }
    public bool CanEditUsers { get; private set; }
    public bool CanViewPermissions { get; private set; }
    public bool CanViewStatuses { get; private set; }
    public bool CanEditStatuses { get; private set; }
    public bool CanViewEquipments { get; private set; }
    public bool CanEditEquipments { get; private set; }
    public bool CanViewAreas { get; private set; }
    public bool CanEditAreas { get; private set; }
    public bool CanViewCities { get; private set; }
    public bool CanEditCities { get; private set; }
    public bool CanViewWorkloads { get; private set; }
    public bool CanEditWorkloads { get; private set; }
    public bool CanCompleteWorkloads { get; private set; }
    public bool CanViewAssignments { get; private set; }
    public bool CanGetAssignments { get; private set; }
    public bool CanEditAssignments { get; private set; }
    public bool CanEditAssignmentsStatuses { get; private set; }
    public bool CanViewAssignmentsHistory { get; private set; }
    public bool CanViewSettings { get; private set; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="permissions">List of permissions</param>
    public UserPermissions(IEnumerable<PermissionDto>? permissions)
    {
        if (permissions != null)
        {
            CanViewUsers = permissions.Any(p => p.PermissionName == nameof(CanViewUsers));
            CanViewPermissions = permissions.Any(p => p.PermissionName == nameof(CanViewPermissions));
            CanEditUsers = permissions.Any(p => p.PermissionName == nameof(CanEditUsers));
            CanViewStatuses = permissions.Any(p => p.PermissionName == nameof(CanViewStatuses));
            CanEditStatuses = permissions.Any(p => p.PermissionName == nameof(CanEditStatuses));
            CanViewEquipments = permissions.Any(p => p.PermissionName == nameof(CanViewEquipments));
            CanEditEquipments = permissions.Any(p => p.PermissionName == nameof(CanEditEquipments));
            CanViewAreas = permissions.Any(p => p.PermissionName == nameof(CanViewAreas));
            CanEditAreas = permissions.Any(p => p.PermissionName == nameof(CanEditAreas));
            CanViewCities = permissions.Any(p => p.PermissionName == nameof(CanViewCities));
            CanEditCities = permissions.Any(p => p.PermissionName == nameof(CanEditCities));
            CanViewWorkloads = permissions.Any(p => p.PermissionName == nameof(CanViewWorkloads));
            CanEditWorkloads = permissions.Any(p => p.PermissionName == nameof(CanEditWorkloads));
            CanCompleteWorkloads = permissions.Any(p => p.PermissionName == nameof(CanCompleteWorkloads));
            CanViewAssignments = permissions.Any(p => p.PermissionName == nameof(CanViewAssignments));
            CanGetAssignments = permissions.Any(p => p.PermissionName == nameof(CanGetAssignments));
            CanEditAssignmentsStatuses = permissions.Any(p => p.PermissionName == nameof(CanEditAssignmentsStatuses));
            CanViewAssignmentsHistory = permissions.Any(p => p.PermissionName == nameof(CanViewAssignmentsHistory));
            CanEditAssignments = permissions.Any(p => p.PermissionName == nameof(CanEditAssignments));
            CanViewSettings = permissions.Any(p => p.PermissionName == nameof(CanViewSettings));
        }
    }
    #endregion
}
