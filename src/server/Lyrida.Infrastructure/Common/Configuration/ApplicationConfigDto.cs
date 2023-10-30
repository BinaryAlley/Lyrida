namespace Lyrida.Infrastructure.Common.Configuration;

/// <summary>
/// Model for strongly typed application configuration values
/// </summary>
/// <remarks>
/// Creation Date: 13th of July, 2021
/// </remarks>
public class ApplicationConfigDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string? Language { get; set; }
    public bool IsProductionMedium { get; set; }
    #endregion
}