namespace Lyrida.Infrastructure.Common.Security;

/// <summary>
/// Security related services
/// </summary>
/// <remarks>
/// Creation Date: 18th of January, 2022
/// </remarks>
public class SecurityService : ISecurity
{
    #region ==================================================================== PROPERTIES =================================================================================
    public IHash HashService { get; }
    public ICryptography CryptographyService { get; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="hash">Injected password hashing service</param>
    /// <param name="cryptography">Injected cryptography service</param>
    public SecurityService(IHash hash, ICryptography cryptography)
    {
        HashService = hash;
        CryptographyService = cryptography;
    }
    #endregion
}