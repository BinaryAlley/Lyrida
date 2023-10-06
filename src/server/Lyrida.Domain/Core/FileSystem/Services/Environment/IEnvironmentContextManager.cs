#region ========================================================================= USING =====================================================================================
using Lyrida.Domain.Common.Enums;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Environment;

/// <summary>
/// Interface for managing the environment context
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
public interface IEnvironmentContextManager
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the current environment context
    /// </summary>
    /// <returns>The current environment context</returns>
    IEnvironmentContext GetCurrentContext();

    /// <summary>
    /// Sets the current environment context
    /// </summary>
    /// <param name="environmentType">The environment to set</param>
    /// <exception cref="ArgumentException">Thrown when an unsupported environment type is provided</exception>
    void SetCurrentEnvironment(EnvironmentType environmentType);
    #endregion
}