namespace Lyrida.Api.Common.DTO.Authentication;

/// <summary>
/// Data transfer object for API login responses
/// </summary>
/// <remarks>
/// Creation Date: 24th of October, 2023
/// </remarks>
public record LoginResponseDto(int Id, string Username, string Token, bool UsesTotp);