#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.Authorization;

/// <summary>
/// UI DTO for permissions
/// </summary>
/// <remarks>
/// Creation Date: 13th of June, 2023
/// </remarks>
[DebuggerDisplay("Id: {Id} PermissionName: {PermissionName}")]
public class PermissionDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public string PermissionName { get; set; } = null!;
    #endregion
}