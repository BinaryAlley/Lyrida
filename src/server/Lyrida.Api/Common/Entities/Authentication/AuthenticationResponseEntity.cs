namespace Lyrida.Api.Common.Entities.Authentication;

/// <summary>
/// Data transfer object for API authentication responses
/// </summary>
/// <remarks>
/// Creation Date: 12th of July, 2023
/// </remarks>
public record AuthenticationResponseEntity(int Id, string FirstName, string LastName, string Email, string Token);