namespace Lyrida.Api.Common.DTO.Authentication;

/// <summary>
/// Data transfer object for API recover password validation requests
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public record RecoverPasswordRequestDto(string Username, int TotpCode);