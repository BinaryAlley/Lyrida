namespace Lyrida.DataAccess.Repositories.Common.Factory;

/// <summary>
/// Repositories abstract factory
/// </summary>
/// <remarks>
/// Creation Date: 12th of June, 2021
/// </remarks>
public interface IRepositoryFactory
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates a new repository of type <typeparamref name="TResult"/>
    /// </summary>
    /// <typeparam name="TResult">The type of repository to create</typeparam>
    /// <returns>A repository of type <typeparamref name="TResult"/></returns>
    TResult CreateRepository<TResult>() where TResult : notnull;
    #endregion
}