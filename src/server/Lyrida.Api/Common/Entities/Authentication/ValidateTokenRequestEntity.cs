namespace Lyrida.Api.Common.Entities.Authentication;

/// <summary>
/// Data transfer object for API register token validation requests
/// </summary>
/// <remarks>
/// Creation Date: 27th of July, 2023
/// </remarks>
public record ValidateTokenRequestEntity(string Token);