#region ========================================================================= USING =====================================================================================
using System;
using Autofac;
#endregion

namespace Lyrida.DataAccess.Repositories.Common.Factory;

/// <summary>
/// Concrete implementation for the repositories abstract factory
/// </summary>
/// <remarks>
/// Creation Date: 12th of June, 2021
/// </remarks>
public class RepositoryFactory : IRepositoryFactory
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ILifetimeScope container;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="container">The DI container that resolves the requested repositories</param>
    public RepositoryFactory(ILifetimeScope container)
    {
        this.container = container;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates a new repository of type <typeparamref name="TResult"/>
    /// </summary>
    /// <typeparam name="TResult">The type of repository to create</typeparam>
    /// <returns>A repository of type <typeparamref name="TResult"/></returns>
    /// <exception cref="ArgumentException">Thrown when the type of the requested repository has not been registered</exception>
    public TResult CreateRepository<TResult>() where TResult : notnull
    {
        return container.Resolve<TResult>() ?? throw new ArgumentException();
    }
    #endregion
}