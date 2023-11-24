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
    string? ServerPath { get; set; }
    int ServerPort { get; set; }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Saves the application's configuration settings
    /// </summary>
    void UpdateConfiguration();
    #endregion
}