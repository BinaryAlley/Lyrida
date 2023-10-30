namespace Lyrida.Application.Common.DTO.Authentication;

/// <summary>
/// Data transfer object for creating TOTP qr codes command
/// </summary>
/// <remarks>
/// Creation Date: 16th of October, 2023
/// </remarks>
public record QrCodeResultDto(string DataUri);