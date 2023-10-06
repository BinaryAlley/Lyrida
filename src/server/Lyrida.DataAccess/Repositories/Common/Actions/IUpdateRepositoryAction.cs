#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Entities.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Common.Actions;

/// <summary>
/// Interface defining the "update" action for interacting with a generic persistance medium
/// </summary>
/// <remarks>
/// Creation Date: 10th of March, 2022
/// </remarks>
/// <typeparam name="TEntity">The type used for the update action. It should implement <see cref="IStorageEntity"/></typeparam>
public interface IUpdateRepositoryAction<TEntity> where TEntity : IStorageEntity
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Updates <paramref name="entity"/> in the storage medium
    /// </summary>
    /// <param name="entity">The entity that will be updated</param>
    /// <returns>The result of updating the <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
    Task<ApiResponse> UpdateAsync(TEntity entity);
    #endregion
}