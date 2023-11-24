namespace Lyrida.Api.Common.DTO.Authentication;

/// <summary>
/// Data transfer object for API login requests
/// </summary>
/// <remarks>
/// Creation Date: 12th of July, 2023
/// </remarks>
public record LoginRequestDto(string Username, string Password, string? TotpCode);