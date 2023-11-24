#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.Authentication;

/// <summary>
/// Account login data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 13th of June, 2023
/// </remarks>
[DebuggerDisplay("Username: {Username}")]
public class LoginDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? TotpCode { get; set; }
    #endregion
}