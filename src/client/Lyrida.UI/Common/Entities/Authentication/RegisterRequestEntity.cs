#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.Entities.Authentication;

/// <summary>
/// Account registration data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 15th of June, 2023
/// </remarks>
[DebuggerDisplay("{Email}")]
public record RegisterRequestEntity(string? FirstName, string? LastName, string? Email, string? Password, string? PasswordConfirm, string? RegistrationType);