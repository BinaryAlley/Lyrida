namespace Lyrida.DataAccess.UoW;

/// <summary>
/// Interaction boundary with the Data Access Layer
/// </summary>
/// <remarks>
/// Creation Date: 11th of June, 2021
/// </remarks>
public interface IUnitOfWork
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Exposes a repository of type <typeparamref name="TRepository"/> to the Business Layer
    /// </summary>
    /// <typeparam name="TRepository">The type of the exposed repository</typeparam>
    /// <returns>A repository of type <typeparamref name="TRepository"/></returns>
    TRepository GetRepository<TRepository>();
    #endregion
}