#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Entities.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Common.Actions;

/// <summary>
/// Interface defining the "insert" action for interacting with a generic persistance medium
/// </summary>
/// <remarks>
/// Creation Date: 10th of March, 2022
/// </remarks>
/// <typeparam name="TEntity">The type used for the insert action. It should implement <see cref="IStorageEntity"/></typeparam>
public interface IInsertRepositoryAction<TEntity> where TEntity : IStorageEntity
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Saves an entity of type <typeparamref name="TEntity"/> in the storage medium
    /// </summary>
    /// <param name="entity">The entity to be saved</param>
    /// <returns>The result of saving <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse{TEntity}"/></returns>
    Task<ApiResponse<TEntity>> InsertAsync(TEntity entity);
    #endregion
}