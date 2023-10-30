#region ========================================================================= USING =====================================================================================
using Autofac;
using Mapster;
using MapsterMapper;
using Module = Autofac.Module;
using Lyrida.Api.Common.Errors;
using Autofac.Extras.DynamicProxy;
using Lyrida.Infrastructure.Common.Logging;
using Microsoft.AspNetCore.Mvc.Infrastructure;
#endregion

namespace Lyrida.Api.Common.DependencyInjection;

/// <summary>
/// Contains all services of the API Layer
/// </summary>
/// <remarks>
/// Creation Date: 04th of July, 2023
/// </remarks>
public class ApiLayerServices : Module
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Registers the services of the API Layer into the Dependency Injection container
    /// </summary>
    /// <param name="builder">The Dependency Injection container where the services are registered</param>
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CustomProblemDetailsFactory>()
               .As<ProblemDetailsFactory>()
#if !DEBUG
               .EnableClassInterceptors()
               .InterceptedBy(typeof(ProxyInterceptor))
#endif
               .SingleInstance();

        // register mapster configs
        var mapsterConfig = TypeAdapterConfig.GlobalSettings;
        mapsterConfig.Scan(typeof(ApiLayerServices).Assembly);

        builder.RegisterInstance(mapsterConfig);

        builder.RegisterType<ServiceMapper>()
               .As<IMapper>()
#if !DEBUG
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ProxyInterceptor))
#endif
               .InstancePerLifetimeScope();
    }
    #endregion
}