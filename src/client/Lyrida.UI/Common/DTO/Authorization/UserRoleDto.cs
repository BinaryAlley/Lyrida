#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.Authorization;

/// <summary>
/// UI DTO for user roles
/// </summary>
/// <remarks>
/// Creation Date: 18th of August, 2023
/// </remarks>
[DebuggerDisplay("Id: {Id} UserId: {UserId} RoleId: {RoleId}")]
public class UserRoleDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    #endregion
}