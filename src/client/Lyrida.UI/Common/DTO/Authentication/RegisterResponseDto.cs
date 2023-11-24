#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.Authentication;

/// <summary>
/// Account registration responses data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 13th of June, 2023
/// </remarks>
[DebuggerDisplay("{Username}")]
public class RegisterResponseDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Token { get; set; }
    public string? TotpSecret { get; set; }
    #endregion
}