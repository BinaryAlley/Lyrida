#region ========================================================================= USING =====================================================================================
using System;
using Autofac;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Files.FileProviderStrategies;

/// <summary>
/// Defines an abstract factory for creating file provider strategies
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
public class FileProviderStrategyFactory : IFileProviderStrategyFactory
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ILifetimeScope container;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="container">The DI container that resolves the requested repositories</param>
    public FileProviderStrategyFactory(ILifetimeScope container)
    {
        this.container = container;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates and returns the appropriate file provider strategy.
    /// </summary>
    /// <typeparam name="TFileProviderStrategy">The type of file provider strategy to create</typeparam>
    /// <returns>The file provider strategy.</returns>
    public TFileProviderStrategy CreateStrategy<TFileProviderStrategy>() where TFileProviderStrategy : IFileProviderStrategy
    {
        return container.Resolve<TFileProviderStrategy>() ?? throw new ArgumentException();
    }
    #endregion
}