namespace Lyrida.Api.Common.Entities.Authentication;

/// <summary>
/// Data transfer object for API password reset requests
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public record ResetPasswordRequestEntity(string Email, string Password, string PasswordConfirm);