namespace Lyrida.Infrastructure.Core.Authentication;

/// <summary>
/// Interface for generating JWT tokens
/// </summary>
/// <remarks>
/// Creation Date: 07th of July, 2023
/// </remarks>
public interface IJwtTokenGenerator
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Generates a new JWT token.
    /// </summary>
    /// <param name="id">The id of the user for which to generate the token.</param>
    /// <param name="username">The username for which to generate the token.</param>
    /// <returns>The generated JWT token.</returns>
    string GenerateToken(string id, string username);
    #endregion
}