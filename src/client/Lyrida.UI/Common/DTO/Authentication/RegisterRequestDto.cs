#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.Authentication;

/// <summary>
/// Account registration data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 15th of June, 2023
/// </remarks>
[DebuggerDisplay("{Username}")]
public class RegisterRequestDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? PasswordConfirm { get; set; }
    public string? RegistrationType { get; set; }
    public bool Use2fa { get; set; } = true;
    #endregion
}