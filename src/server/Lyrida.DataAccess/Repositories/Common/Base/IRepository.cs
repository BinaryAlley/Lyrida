#region ========================================================================= USING =====================================================================================
using Lyrida.DataAccess.StorageAccess;
#endregion

namespace Lyrida.DataAccess.Repositories.Common.Base;

/// <summary>
/// Interface for interaction with a generic persistance medium
/// </summary>
/// <remarks>
/// Creation Date: 11th of June, 2021
/// </remarks>
/// <typeparam name="TEntity">The type used for the repository collection. It should implement <see cref="IStorageEntity"/></typeparam>
public interface IRepository<TEntity> where TEntity : IStorageEntity
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Opens a transaction
    /// </summary>
    void OpenTransaction();

    /// <summary>
    /// Closes a transaction, rolls back changes if the transaction was faulty
    /// </summary>
    void CloseTransaction();
    #endregion
}