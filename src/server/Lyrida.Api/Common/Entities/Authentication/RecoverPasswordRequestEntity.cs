namespace Lyrida.Api.Common.Entities.Authentication;

/// <summary>
/// Data transfer object for API recover password validation requests
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public record RecoverPasswordRequestEntity(string Email);