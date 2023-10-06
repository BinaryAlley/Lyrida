#region ========================================================================= USING =====================================================================================
using System;
using System.Text;
using System.Security.Cryptography;
#endregion

namespace Lyrida.Infrastructure.Common.Security;

/// <summary>
/// Handles encryption and decryption of strings using AES128 
/// </summary>
/// <remarks>
/// Creation Date: 30th of March, 2023
/// </remarks>
public class Cryptography : ICryptography
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    // TODO: change key and IV in production, remove credentials!

    private const string KEY = "lkirwf897+22#bbtrm8814z5qq=498j5"; // 32 char shared ASCII string (32 * 8 = 256 bit)
    private const string INITIALIZATION_VECTOR = "6#cs!9hjv887mx7@"; // 16 char shared ASCII string (16 * 8 = 128 bit)
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Encrypts a string
    /// </summary>
    /// <param name="param">The string to exncrypt</param>
    /// <returns>The encrypted string</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="param"/> is null or empty</exception>
    public string Encrypt(string param)
    {
        if (!string.IsNullOrEmpty(param))
        {
            byte[]? sEncrypted = Encoding.UTF8.GetBytes(param);
            byte[]? encrypted = CreateAes().CreateEncryptor().TransformFinalBlock(sEncrypted, 0, sEncrypted.Length);
            return Convert.ToBase64String(encrypted);
        }
        else
            throw new ArgumentException("Parameter cannot be null or empty!");
    }

    /// <summary>
    /// Decrypts a string
    /// </summary>
    /// <param name="param">The string to decrypt</param>
    /// <returns>The decrypted string</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="param"/> is null or empty</exception>
    public string Decrypt(string param)
    {
        if (!string.IsNullOrEmpty(param))
        {
            byte[]? sEncrypted = Convert.FromBase64String(param);
            byte[]? decrypted = CreateAes().CreateDecryptor().TransformFinalBlock(sEncrypted, 0, sEncrypted.Length);
            return Encoding.UTF8.GetString(decrypted);
        }
        else
            throw new ArgumentException("Parameter cannot be null or empty!");
    }

    /// <summary>
    /// Creates and initializes a new cryptographic object that is used to perform the symmetric algorithm
    /// </summary>
    /// <returns>A cryptographic object that is used to perform the symmetric algorithm</returns>
    private static Aes CreateAes()
    {
        Aes aes = Aes.Create();
        aes.KeySize = 128;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = Encoding.ASCII.GetBytes(KEY);
        aes.IV = Encoding.ASCII.GetBytes(INITIALIZATION_VECTOR);
        return aes;
    }
    #endregion
}