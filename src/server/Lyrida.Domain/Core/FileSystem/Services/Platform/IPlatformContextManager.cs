#region ========================================================================= USING =====================================================================================
using Lyrida.Domain.Common.Enums;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Platform;

/// <summary>
/// Interface for managing the platform context
/// </summary>
/// <remarks>
/// Creation Date: 04th of September, 2023
/// </remarks>
public interface IPlatformContextManager
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the current platform context
    /// </summary>
    /// <returns>The current platform context</returns>
    IPlatformContext GetCurrentContext();

    /// <summary>
    /// Sets the current platform context
    /// </summary>
    /// <param name="platformType">The platform to set</param>
    /// <exception cref="ArgumentException">Thrown when an unsupported platform type is provided</exception>
    void SetCurrentPlatform(PlatformType platformType);
    #endregion
}