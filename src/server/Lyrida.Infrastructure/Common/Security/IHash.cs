namespace Lyrida.Infrastructure.Common.Security;

/// <summary>
/// Interface for hashing passwords
/// </summary>
/// <remarks>
/// Creation Date: 30th of July, 2018
/// </remarks>
public interface IHash
{
    #region ===================================================================== METHODS ===================================================================================        
    /// <summary>
    /// Creates a hash from a string
    /// </summary>
    /// <param name="password">The string to be hashed</param>
    /// <returns>The hashed string</returns>
    string HashString(string password);

    /// <summary>
    /// Verifies a password against a hash
    /// </summary>
    /// <param name="password">The string to be checked</param>
    /// <param name="hashedPassword">The hashed representation of the string to be checked</param>
    /// <returns>True if <paramref name="password"/> and the de-hashed verions of <paramref name="hashedPassword"/> are equal, False otherwise</returns>
    bool CheckStringAgainstHash(string password, string hashedPassword);
    #endregion
}