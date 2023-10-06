#region ========================================================================= USING =====================================================================================
using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
#endregion

namespace Lyrida.UI.Common.DependencyInjection;

/// <summary>
/// Factory for creating child lifetime scopes with their own service provider based on the root lifetime scope
/// </summary>
/// <remarks>
/// Creation Date: 15th of June, 2023
/// </remarks>
public class ChildScopeServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ILifetimeScope rootScope;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="rootScope">The root lifetime scope of the application.</param>
    public ChildScopeServiceProviderFactory(ILifetimeScope rootScope)
    {
        this.rootScope = rootScope;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates a new instance of the ContainerBuilder and populates it with services.
    /// </summary>
    /// <param name="services">The collection of services to be registered.</param>
    /// <returns>The ContainerBuilder instance.</returns>
    public ContainerBuilder CreateBuilder(IServiceCollection services)
    {
        var builder = new ContainerBuilder();
        builder.Populate(services);
        return builder;
    }

    /// <summary>
    /// Creates a new instance of the IServiceProvider using the ContainerBuilder.
    /// </summary>
    /// <param name="containerBuilder">The ContainerBuilder instance.</param>
    /// <returns>The IServiceProvider instance.</returns>
    public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
    {
        // Creates a new lifetime scope named "userSession" with the given container configuration.
        return new AutofacServiceProvider(rootScope.BeginLifetimeScope("userSession", b => containerBuilder.Build()));
    }
    #endregion
}