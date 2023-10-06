namespace Lyrida.Infrastructure.Common.Security;

/// <summary>
/// Interface for cryptography service
/// </summary>
/// <remarks>
/// Creation Date: 20th of October, 2019
/// </remarks>
public interface ICryptography
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Encrypts a string
    /// </summary>
    /// <param name="param">The string to exncrypt</param>
    /// <returns>The encrypted string</returns>
    string Encrypt(string param);

    /// <summary>
    /// Decrypts a string
    /// </summary>
    /// <param name="param">The string to decrypt</param>
    /// <returns>The decrypted string</returns>
    string Decrypt(string param);
    #endregion
}