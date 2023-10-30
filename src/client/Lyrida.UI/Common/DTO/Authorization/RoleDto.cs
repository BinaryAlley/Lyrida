#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.Authorization;

/// <summary>
/// UI DTO for roles
/// </summary>
/// <remarks>
/// Creation Date: 10th of August, 2023
/// </remarks>
[DebuggerDisplay("Id: {Id} RoleName: {RoleName}")]
public class RoleDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public string RoleName { get; set; } = null!;
    #endregion
}