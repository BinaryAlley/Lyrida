namespace Lyrida.Api.Common.DTO.Authentication;

/// <summary>
/// Data transfer object for API register token validation requests
/// </summary>
/// <remarks>
/// Creation Date: 27th of July, 2023
/// </remarks>
public record ValidateTokenRequestDto(string Token);