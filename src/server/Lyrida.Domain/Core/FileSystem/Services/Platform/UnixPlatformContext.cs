#region ========================================================================= USING =====================================================================================
using Lyrida.Domain.Common.Enums;
using Lyrida.Domain.Core.FileSystem.Services.Paths.PathStrategies;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Platform;

/// <summary>
/// Platform context for UNIX platforms
/// </summary>
/// <remarks>
/// Creation Date: 04th of September, 2023
/// </remarks>
internal class UnixPlatformContext : IUnixPlatformContext
{
    #region ==================================================================== PROPERTIES =================================================================================
    public PlatformType Platform { get; } = PlatformType.Unix;
    public IPathStrategy PathService { get; private set; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unixPathStrategy">Injected service for creating path strategies for UNIX platforms</param>
    public UnixPlatformContext(IUnixPathStrategy unixPathStrategy)
    {
        PathService = unixPathStrategy;
    }
    #endregion
}