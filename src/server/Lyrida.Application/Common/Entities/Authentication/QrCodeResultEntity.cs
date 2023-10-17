namespace Lyrida.Application.Common.Entities.Authentication;

/// <summary>
/// Data transfer object for creating TOTP qr codes command
/// </summary>
/// <remarks>
/// Creation Date: 16th of October, 2023
/// </remarks>
public record QrCodeResultEntity(string DataUri);