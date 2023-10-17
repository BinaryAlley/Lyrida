namespace Lyrida.Infrastructure.Core.Authentication;

/// <summary>
/// Interface for generating and validating TOTP tokens.
/// </summary>
/// <remarks>
/// Creation Date: 16th of October, 2023
/// </remarks>
public interface ITotpTokenGenerator
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Generates a new TOTP secret for a user.
    /// </summary>
    /// <returns>The generated TOTP secret.</returns>
    byte[] GenerateSecret();

    /// <summary>
    /// Validates a TOTP token against the stored secret.
    /// </summary>
    /// <param name="secret">The user's TOTP secret.</param>
    /// <param name="token">The token to validate.</param>
    /// <returns><see langword="true"/> if valid, <see langword="false"/> otherwise.</returns>
    bool ValidateToken(byte[] secret, string token);
    #endregion
}
