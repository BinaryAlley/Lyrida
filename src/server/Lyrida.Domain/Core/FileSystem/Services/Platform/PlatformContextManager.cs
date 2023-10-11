#region ========================================================================= USING =====================================================================================
using System;
using Lyrida.Domain.Common.Enums;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Platform;

/// <summary>
/// Service for managing the platform context
/// </summary>
/// <remarks>
/// Creation Date: 04th of September, 2023
/// </remarks>
public class PlatformContextManager : IPlatformContextManager
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private IPlatformContext? currentPlatformContext;
    private readonly IPlatformContextFactory platformContextFactory;
    #endregion

   

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="platformContextFactory">Injected abstract factory for creating platform contexts</param>
    public PlatformContextManager(IPlatformContextFactory platformContextFactory)
    {
        this.platformContextFactory = platformContextFactory;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the current platform context
    /// </summary>
    /// <returns>The current platform context</returns>
    public IPlatformContext GetCurrentContext()
    {
        // set a default context if none is set
        if (currentPlatformContext is null)
            SetCurrentPlatform(PlatformType.Unix);
        return currentPlatformContext!;
    }

    /// <summary>
    /// Sets the current platform context
    /// </summary>
    /// <param name="platformType">The platform to set</param>
    /// <exception cref="ArgumentException">Thrown when an unsupported platform type is provided</exception>
    public void SetCurrentPlatform(PlatformType platformType)
    {
        // determine the correct context based on platformType
        currentPlatformContext = platformType switch
        {
            PlatformType.Unix => platformContextFactory.CreateStrategy<IUnixPlatformContext>(),
            PlatformType.Windows => platformContextFactory.CreateStrategy<IWindowsPlatformContext>(),
            _ => throw new ArgumentException($"Unsupported platform type: {platformType}"),
        };
    }
    #endregion
}