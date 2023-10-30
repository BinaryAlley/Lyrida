#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.Authorization;

/// <summary>
/// UI DTO for user permissions
/// </summary>
/// <remarks>
/// Creation Date: 19th of August, 2023
/// </remarks>
[DebuggerDisplay("Id: {Id} UserId: {UserId} PermissionId: {PermissionId}")]
public class UserPermissionDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = null!;
    #endregion
}