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
[DebuggerDisplay("{Username}")]
public class RecoverPasswordDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string? Username { get; set; }
    public string? Token { get; set; }
    public string? Password { get; set; }
    public string? PasswordConfirm { get; set; }
    #endregion
}