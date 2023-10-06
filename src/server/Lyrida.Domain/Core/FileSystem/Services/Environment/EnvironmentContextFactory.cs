#region ========================================================================= USING =====================================================================================
using System;
using Autofac;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Environment;

/// <summary>
/// Defines an abstract factory for creating environment contexts
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
public class EnvironmentContextFactory : IEnvironmentContextFactory
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ILifetimeScope container;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="container">The DI container that resolves the requested repositories</param>
    public EnvironmentContextFactory(ILifetimeScope container)
    {
        this.container = container;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates and returns the appropriate environment context.
    /// </summary>
    /// <typeparam name="TEnvironmentContext">The type of environment context to create</typeparam>
    /// <returns>The environment context.</returns>
    public TEnvironmentContext CreateStrategy<TEnvironmentContext>() where TEnvironmentContext : IEnvironmentContext
    {
        return container.Resolve<TEnvironmentContext>() ?? throw new ArgumentException();
    }
    #endregion
}