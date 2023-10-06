namespace Lyrida.Infrastructure.Common.Security;

/// <summary>
/// Interface for the security related services
/// </summary>
/// <remarks>
/// Creation Date: 18th of January, 2022
/// </remarks>
public interface ISecurity
{
    #region ==================================================================== PROPERTIES =================================================================================
    IHash HashService { get; }
    ICryptography CryptographyService { get; }
    #endregion
}