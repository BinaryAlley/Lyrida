#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.Authentication;

/// <summary>
/// Account password reset data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 13th of June, 2023
/// </remarks>
[DebuggerDisplay("{Email}")]
public class RecoverPasswordDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string? Email { get; set; }
    public string? Token { get; set; }
    public string? Password { get; set; }
    public string? PasswordConfirm { get; set; }
    #endregion
}