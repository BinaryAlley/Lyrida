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
[DebuggerDisplay("{Email}")]
public class LoginResponseDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
    public bool UsesTotp { get; set; }
    #endregion
}