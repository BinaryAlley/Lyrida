#region ========================================================================= USING =====================================================================================
using System;
using Autofac;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Directories.DirectoryProviderStrategies;

/// <summary>
/// Defines an abstract factory for creating directory provider strategies
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
public class DirectoryProviderStrategyFactory : IDirectoryProviderStrategyFactory
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ILifetimeScope container;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="container">The DI container that resolves the requested repositories</param>
    public DirectoryProviderStrategyFactory(ILifetimeScope container)
    {
        this.container = container;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates and returns the appropriate directory provider strategy.
    /// </summary>
    /// <typeparam name="TDirectoryProviderStrategy">The type of directory provider strategy to create</typeparam>
    /// <returns>The directory provider strategy.</returns>
    public TDirectoryProviderStrategy CreateStrategy<TDirectoryProviderStrategy>() where TDirectoryProviderStrategy : IDirectoryProviderStrategy
    {
        return container.Resolve<TDirectoryProviderStrategy>() ?? throw new ArgumentException();
    }
    #endregion
}