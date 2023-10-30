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
[DebuggerDisplay("{Email}")]
public class RegisterRequestDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PasswordConfirm { get; set; }
    public string? RegistrationType { get; set; }
    public bool Use2fa { get; set; } = true;
    #endregion
}