namespace Lyrida.Api.Common.DTO.Authentication;

/// <summary>
/// Data transfer object for API register requests
/// </summary>
/// <remarks>
/// Creation Date: 12th of July, 2023
/// </remarks>
public record RegisterRequestDto(string FirstName, string LastName, string Email, string Password, string PasswordConfirm, bool Use2fa);