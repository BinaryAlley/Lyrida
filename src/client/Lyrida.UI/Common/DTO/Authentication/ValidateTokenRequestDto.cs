#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.DTO.Authentication;

/// <summary>
/// Account registration token validation data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 27th of July, 2023
/// </remarks>
[DebuggerDisplay("{Token}")]
public class ValidateTokenRequestDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string? Token { get; set; }
    #endregion
}