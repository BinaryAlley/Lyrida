namespace Lyrida.Api.Common.Entities.Authentication;

/// <summary>
/// Data transfer object for API login requests
/// </summary>
/// <remarks>
/// Creation Date: 12th of July, 2023
/// </remarks>
public record LoginRequestEntity(string Email, string Password);