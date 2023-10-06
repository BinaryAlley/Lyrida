namespace Lyrida.Infrastructure.Core.Authentication;

/// <summary>
/// Interface for the generating secure tokens
/// </summary>
/// <remarks>
/// Creation Date: 20th of June, 2023
/// </remarks>
public interface ITokenGenerator
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Generates a secure token.
    /// </summary>
    /// <returns>A secure token.</returns>
    string GenerateToken();
    #endregion
}