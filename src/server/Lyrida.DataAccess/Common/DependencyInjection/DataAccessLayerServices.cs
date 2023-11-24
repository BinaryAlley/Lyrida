#region ========================================================================= USING =====================================================================================
using System;
using Autofac;
using System.Data;
using System.Linq;
using System.Reflection;
using Lyrida.DataAccess.UoW;
using System.Collections.Generic;
using Autofac.Extras.DynamicProxy;
using Lyrida.Infrastructure.Common.Logging;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Infrastructure.Common.DependencyInjection;
using Microsoft.Extensions.Configuration;
#endregion

namespace Lyrida.DataAccess.Common.DependencyInjection;

/// <summary>
/// Contains all services of the Data Access Layer
/// </summary>
/// <remarks>
/// Creation Date: 10th of July, 2023
/// </remarks>
public class DataAccessLayerServices : Autofac.Module
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Registers the services of the Infrastructure Layer into the Dependency Injection container
    /// </summary>
    /// <param name="builder">The Dependency Injection container where the services are registered</param>
    protected override void Load(ContainerBuilder builder)
    {
        //builder.RegisterType<DateTimeProvider>().As<IDateTimeProvider>().SingleInstance();
        builder.RegisterType<UnitOfWork>()
               .As<IUnitOfWork>()
               //.SingleInstance();
               .InstancePerLifetimeScope();

        Type[]? dataAccessLayerTypes = Assembly.GetExecutingAssembly().GetTypes();
        Type? iDataAccessType = Type.GetType("Lyrida.DataAccess.StorageAccess.IDataAccess");
        Type? sqlDataAccessType = Type.GetType("Lyrida.DataAccess.StorageAccess.SqlDataAccess");
        Type? mySqlConnectionType = Type.GetType("MySqlConnector.MySqlConnection, MySqlConnector");
        Type? genericRepositoryType = Type.GetType("Lyrida.DataAccess.Repositories.Common.Base.IRepository`1");
        Type? repositoryFactoryType = Type.GetType("Lyrida.DataAccess.Repositories.Common.Factory.RepositoryFactory");
        Type? iRepositoryFactoryType = Type.GetType("Lyrida.DataAccess.Repositories.Common.Factory.IRepositoryFactory");

        if (sqlDataAccessType != null && iDataAccessType != null)
        {
            builder.RegisterType(sqlDataAccessType)
                   .FindConstructorsWith(new InternalConstructorFinder())
                   .As(iDataAccessType)
                   .OnActivating(e =>
                   {
                       Type? instanceType = e.Instance?.GetType();
                       var configuration = e.Context.Resolve<IConfiguration>();
                       instanceType?.GetProperty("ConnectionStringFactory")
                                   ?.SetValue(e.Instance,
                                         () => configuration.GetConnectionString(configuration.GetSection("Application").GetValue<bool>("IsProductionMedium") ? "production" : "test")!);
                   })
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(DatabaseProxyInterceptor))
                   .InstancePerDependency();
        }
        else
            throw new InvalidOperationException("Cannot register data access type!");
        builder.RegisterType(mySqlConnectionType!)
               .As<IDbConnection>()
               .InstancePerDependency(); // needs to be one instance per repository, otherwise parallel tasks from different repositories would try to use the same DataReader
        builder.RegisterType(repositoryFactoryType!)
               .As(iRepositoryFactoryType!)
               .InstancePerLifetimeScope();
        // get all classes implementing IRepository (all repository classes) and register them as their corresponding repository interface
        IEnumerable<Type> repositoryTypes = dataAccessLayerTypes.Where(t => !t.IsInterface &&
                                                                             t.GetInterfaces()
                                                                              .Any(i => i.IsGenericType &&
                                                                                        i.GetGenericTypeDefinition() == genericRepositoryType));
        foreach (Type type in repositoryTypes)
        {
            builder.RegisterType(type)
                   .As(type.GetInterfaces()
                           .Where(i => !i.IsGenericType &&
                                        i.GetInterfaces()
                                         .Any(j => j.GetGenericTypeDefinition() == genericRepositoryType))
                           .First())
                   .InstancePerLifetimeScope();
        }
    }
    #endregion
}