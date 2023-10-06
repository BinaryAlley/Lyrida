namespace Lyrida.Application.Common.Entities.Authentication;

/// <summary>
/// Data transfer object for authentication commands and queries results
/// </summary>
/// <remarks>
/// Creation Date: 12th of July, 2023
/// </remarks>
public record AuthenticationResultEntity(UserEntity User, string Token);