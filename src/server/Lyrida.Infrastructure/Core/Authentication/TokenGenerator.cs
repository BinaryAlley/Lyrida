#region ========================================================================= USING =====================================================================================
using System;
using System.Security.Cryptography;
#endregion

namespace Lyrida.Infrastructure.Core.Authentication;

/// <summary>
/// Service for the generating secure tokens
/// </summary>
/// <remarks>
/// Creation Date: 20th of June, 2023
/// </remarks>
public class TokenGenerator : ITokenGenerator
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Generates a secure token.
    /// </summary>
    /// <returns>A secure token.</returns>
    public string GenerateToken()
    {
        // allocate array for the token data.
        byte[] tokenData = new byte[32];
        // fill the array with a cryptographically strong sequence of random values.
        RandomNumberGenerator.Fill(tokenData);
        // convert to a Base64 string, but replace URL-unsafe characters '+' and '/' with '-' and '_', and remove padding characters ('=').
        return Convert.ToBase64String(tokenData).Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }
    #endregion    
}