namespace Lyrida.Api.Common.DTO.Authentication;

/// <summary>
/// Data transfer object for API change password requests
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public record ChangePasswordRequestDto(string Username, string CurrentPassword, string NewPassword, string NewPasswordConfirm);