#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.Authentication;

/// <summary>
/// Account password change data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
[DebuggerDisplay("{Email}")]
public class ChangePasswordRequestDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string? Email { get; set; }
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? NewPasswordConfirm { get; set; }
    #endregion
}