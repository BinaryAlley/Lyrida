#region ========================================================================= USING =====================================================================================
using OtpNet;
#endregion

namespace Lyrida.Infrastructure.Core.Authentication;

/// <summary>
/// Generates and validates TOTP tokens.
/// </summary>
/// <remarks>
/// Creation Date: 16th of October, 2023
/// </remarks>
public class TotpTokenGenerator : ITotpTokenGenerator
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Generates a new TOTP secret for a user.
    /// </summary>
    /// <returns>The generated TOTP secret.</returns>
    public byte[] GenerateSecret()
    {
        var secret = KeyGeneration.GenerateRandomKey();  // generate a random key for TOTP
        return secret;
    }

    /// <summary>
    /// Validates a TOTP token against the stored secret.
    /// </summary>
    /// <param name="secret">The user's TOTP secret.</param>
    /// <param name="token">The token to validate.</param>
    /// <returns><see langword="true"/> if valid, <see langword="false"/> otherwise.</returns>
    public bool ValidateToken(byte[] secret, string token)
    {
        var totp = new Totp(secret);
        // verify the TOTP token, allowing for slight time drifts between server and client
        return totp.VerifyTotp(token, out _, VerificationWindow.RfcSpecifiedNetworkDelay);
    }
    #endregion
}