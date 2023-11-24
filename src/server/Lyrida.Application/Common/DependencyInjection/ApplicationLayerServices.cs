#region ========================================================================= USING =====================================================================================
using Autofac;
using Mapster;
using MediatR;
using MapsterMapper;
using FluentValidation;
using Autofac.Extras.DynamicProxy;
using Lyrida.Application.Common.Behaviors;
using Lyrida.Infrastructure.Common.Logging;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
#endregion

namespace Lyrida.Application.Common.DependencyInjection;

/// <summary>
/// Contains all services of the Application Layer
/// </summary>
/// <remarks>
/// Creation Date: 04th of July, 2023
/// </remarks>
public class ApplicationLayerServices : Module
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Registers the services of the Application Layer into the Dependency Injection container.
    /// </summary>
    /// <param name="builder">The Dependency Injection container where the services are registered.</param>
    protected override void Load(ContainerBuilder builder)
    {
        // automatically register all mediator commands and queries in this assembly
        var configuration = MediatRConfigurationBuilder.Create(typeof(ApplicationLayerServices).Assembly)
                                                       .WithAllOpenGenericHandlerTypesRegistered()
                                                       .WithRegistrationScope(RegistrationScope.Scoped)
                                                       .Build();
        builder.RegisterMediatR(configuration);

        // register mapster configs in this assembly
        var mapsterConfig = TypeAdapterConfig.GlobalSettings;
        mapsterConfig.Scan(typeof(ApplicationLayerServices).Assembly);

        builder.RegisterInstance(mapsterConfig);

        builder.RegisterType<ServiceMapper>()
               .As<IMapper>()
#if !DEBUG
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ProxyInterceptor))
#endif
               .InstancePerLifetimeScope();

        // register the behaviors of commands
        // notice: before MediatR invokes our handlers, it wraps them in whoever behavior implements the IPipelineBehavior for the types
        // that correspond to the type of the request that its executing (ex: ValidateRegisterCommandBehavior)        
        builder.RegisterGeneric(typeof(ValidationBehavior<,>))
               .As(typeof(IPipelineBehavior<,>))
               .InstancePerLifetimeScope();

        // register fluent validators in this assembly        
        AssemblyScanner.FindValidatorsInAssembly(typeof(ApplicationLayerServices).Assembly)
                       .ForEach(x => builder.RegisterType(x.ValidatorType)
                                            .As(x.InterfaceType)
                                            .InstancePerLifetimeScope());
    }
    #endregion
}