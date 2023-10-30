#region ========================================================================= USING =====================================================================================
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
#endregion

namespace Lyrida.Infrastructure.Common.Configuration;

/// <summary>
/// Model for strongly typed application configuration values
/// </summary>
/// <remarks>
/// Creation Date: 14th of July, 2023
/// </remarks>
public sealed class AppConfig : IAppConfig
{
    #region ==================================================================== PROPERTIES =================================================================================
    public JwtSettingsConfigDto? JwtSettings { get; set; }
    public ApplicationConfigDto? Application { get; set; }
    public Dictionary<string, string>? EmailServer { get; set; }
    public Dictionary<string, string>? DatabaseConnectionStrings { get; set; }
    [JsonIgnore]
    public string? ConfigurationFilePath { get; set; }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Saves the application's configuration settings
    /// </summary>
    public void UpdateConfiguration()
    {
        if (!string.IsNullOrEmpty(ConfigurationFilePath) && File.Exists(ConfigurationFilePath))
            File.WriteAllText(ConfigurationFilePath, JsonConvert.SerializeObject(this, Formatting.Indented));
        else
            throw new InvalidOperationException("Application configuration file was not found!");
    }
    #endregion
}