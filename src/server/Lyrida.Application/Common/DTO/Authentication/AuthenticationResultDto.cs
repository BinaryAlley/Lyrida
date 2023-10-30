namespace Lyrida.Application.Common.DTO.Authentication;

/// <summary>
/// Data transfer object for authentication commands and queries results
/// </summary>
/// <remarks>
/// Creation Date: 12th of July, 2023
/// </remarks>
public record AuthenticationResultDto(UserDto User, string Token);