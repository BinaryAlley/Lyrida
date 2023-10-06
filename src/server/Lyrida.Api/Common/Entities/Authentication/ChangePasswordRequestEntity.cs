namespace Lyrida.Api.Common.Entities.Authentication;

/// <summary>
/// Data transfer object for API change password requests
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public record ChangePasswordRequestEntity(string Email, string CurrentPassword, string NewPassword, string NewPasswordConfirm);