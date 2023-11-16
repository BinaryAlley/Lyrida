#region ========================================================================= USING =====================================================================================
using Lyrida.Domain.Common.Enums;
using Lyrida.Domain.Core.FileSystem.Services.Paths.PathStrategies;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Platform;

/// <summary>
/// Platform context for Windows platforms
/// </summary>
/// <remarks>
/// Creation Date: 04th of September, 2023
/// </remarks>
public class WindowsPlatformContext : IWindowsPlatformContext
{
    #region ==================================================================== PROPERTIES =================================================================================
    public PlatformType Platform { get; } = PlatformType.Windows;
    public IPathStrategy PathStrategy { get; private set; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="windowsPathStrategy">Injected service for creating path strategies for Windows platforms</param>
    public WindowsPlatformContext(IWindowsPathStrategy windowsPathStrategy)
    {
        PathStrategy = windowsPathStrategy;
    }
    #endregion
}