namespace Lyrida.Api.Common.Entities.Authentication;

/// <summary>
/// Data transfer object for API register requests
/// </summary>
/// <remarks>
/// Creation Date: 12th of July, 2023
/// </remarks>
public record RegisterRequestEntity(string FirstName, string LastName, string Email, string Password, string PasswordConfirm);