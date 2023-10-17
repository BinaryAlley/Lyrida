#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.Entities.Authentication;

/// <summary>
/// Account password change data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
[DebuggerDisplay("{Email}")]
public record ChangePasswordRequestEntity(string? Email, string? CurrentPassword, string? NewPassword, string? NewPasswordConfirm);