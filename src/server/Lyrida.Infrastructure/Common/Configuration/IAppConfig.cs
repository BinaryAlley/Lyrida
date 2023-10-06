#region ========================================================================= USING =====================================================================================
using System.Collections.Generic;
#endregion

namespace Lyrida.Infrastructure.Common.Configuration;

/// <summary>
/// Interface for the application's configuration
/// </summary>
/// <remarks>
/// Creation Date: 14th of July, 2023
/// </remarks>
public interface IAppConfig
{
    #region ==================================================================== PROPERTIES =================================================================================
    string? ConfigurationFilePath { get; }
    JwtSettingsConfigEntity? JwtSettings { get; set; }
    ApplicationConfigEntity? Application { get; set; }
    Dictionary<string, string>? EmailServer { get; set; }
    Dictionary<string, string>? DatabaseConnectionStrings { get; set; }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Saves the application's configuration settings
    /// </summary>
    void UpdateConfiguration();
    #endregion
}