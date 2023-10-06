namespace Lyrida.Infrastructure.Common.Configuration;

/// <summary>
/// Model for strongly typed JWT configuration values
/// </summary>
/// <remarks>
/// Creation Date: 11th of July, 2023
/// </remarks>
public sealed class JwtSettingsConfigEntity
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string? SecretKey { get; set; }
    public int ExpiryMinutes { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    #endregion
}