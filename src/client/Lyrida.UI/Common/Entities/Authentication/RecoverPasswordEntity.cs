#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.Entities.Authentication;

/// <summary>
/// Account password reset data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 13th of June, 2023
/// </remarks>
[DebuggerDisplay("{Email}")]
public record RecoverPasswordEntity(string? Email, string? Token, string? Password, string? PasswordConfirm);