#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.Authentication;

/// <summary>
/// Account login responses data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 27th of July, 2023
/// </remarks>
[DebuggerDisplay("{Username}")]
public class LoginResponseDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Token { get; set; }
    public bool UsesTotp { get; set; }
    #endregion
}