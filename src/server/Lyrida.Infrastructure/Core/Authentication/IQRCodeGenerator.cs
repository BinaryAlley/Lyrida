namespace Lyrida.Infrastructure.Core.Authentication;

/// <summary>
/// Interface for generating QR codes for TOTP.
/// </summary>
/// <remarks>
/// Creation Date: 16th of October, 2023
/// </remarks>
public interface IQRCodeGenerator
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Generates a QR code Data URI for TOTP secret.
    /// </summary>
    /// <param name="username">User's username or unique identifier.</param>
    /// <param name="secret">The TOTP secret for which the QR code should be generated.</param>
    /// <returns>Data URI for the QR code.</returns>
    string GenerateQrCodeDataUri(string username, byte[] secret);
    #endregion
}