#region ========================================================================= USING =====================================================================================
using System;
using System.Text;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
#endregion

namespace Lyrida.Infrastructure.Common.Security;

/// <summary>
/// Handles hashing and salting of passwords
/// </summary>
/// <remarks>
/// Creation Date: 30th of July, 2018
/// </remarks>
public sealed class Hash : IHash
{
    #region ===================================================================== METHODS =================================================================================== 
    /// <summary>
    /// Creates a hash from a string.
    /// </summary>
    /// <param name="password">The string to be hashed.</param>
    /// <returns>The hashed string.</returns>
    public string HashString(string password)
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(salt); // generate a random salt
        byte[] hashBytes;
        using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(password)))
        {
            hasher.Salt = salt;
            hasher.DegreeOfParallelism = 8; // set degree of parallelism
            hasher.MemorySize = 65536; // set memory size
            hasher.Iterations = 4; // set number of iterations
            hashBytes = hasher.GetBytes(32); // get 32 bytes hash
        }
        // concatenate salt and hash and return as a Base64 string
        byte[] hashWithSaltBytes = new byte[salt.Length + hashBytes.Length];
        Array.Copy(salt, 0, hashWithSaltBytes, 0, salt.Length);
        Array.Copy(hashBytes, 0, hashWithSaltBytes, salt.Length, hashBytes.Length);
        return Convert.ToBase64String(hashWithSaltBytes);
    }

    /// <summary>
    /// Verifies a password against a hash.
    /// </summary>
    /// <param name="password">The string to be checked.</param>
    /// <param name="hashedPassword">The hashed representation of the string to be checked.</param>
    /// <returns><see langword="true"/> if <paramref name="password"/> and the de-hashed verions of <paramref name="hashedPassword"/> are equal, <see langword="false"/> otherwise.</returns>
    public bool CheckStringAgainstHash(string password, string hashedPassword)
    {
        // convert the Base64 string back into a byte array
        byte[] hashWithSaltBytes = Convert.FromBase64String(hashedPassword);
        // get the salt from the stored hashed password
        byte[] saltBytes = new byte[16];
        Array.Copy(hashWithSaltBytes, 0, saltBytes, 0, saltBytes.Length);
        byte[] hashBytes;
        using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(password)))
        {
            hasher.Salt = saltBytes;
            hasher.DegreeOfParallelism = 8; // set degree of parallelism
            hasher.MemorySize = 65536; // set memory size
            hasher.Iterations = 4; // set number of iterations
            hashBytes = hasher.GetBytes(32); // get 32 bytes hash
        }
        // get the stored hash from the stored hashed password
        byte[] storedHashBytes = new byte[hashWithSaltBytes.Length - saltBytes.Length];
        Array.Copy(hashWithSaltBytes, saltBytes.Length, storedHashBytes, 0, storedHashBytes.Length);
        // compare computed hash with stored hash
        return Convert.ToBase64String(hashBytes) == Convert.ToBase64String(storedHashBytes);
    }
    #endregion
}