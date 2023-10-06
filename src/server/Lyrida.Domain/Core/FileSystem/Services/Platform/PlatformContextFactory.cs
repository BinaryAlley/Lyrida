#region ========================================================================= USING =====================================================================================
using System;
using Autofac;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Platform;

/// <summary>
/// Defines an abstract factory for creating platform contexts
/// </summary>
/// <remarks>
/// Creation Date: 04th of September, 2023
/// </remarks>
public class PlatformContextFactory : IPlatformContextFactory
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ILifetimeScope container;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="container">The DI container that resolves the requested repositories</param>
    public PlatformContextFactory(ILifetimeScope container)
    {
        this.container = container;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates and returns the appropriate platform context.
    /// </summary>
    /// <typeparam name="TPlatformContext">The type of platform context to create</typeparam>
    /// <returns>The platform context.</returns>
    public TPlatformContext CreateStrategy<TPlatformContext>() where TPlatformContext : IPlatformContext
    {
        return container.Resolve<TPlatformContext>() ?? throw new ArgumentException();
    }
    #endregion
}