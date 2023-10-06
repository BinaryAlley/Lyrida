#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Entities.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Common.Actions;

/// <summary>
/// Interface defining the "get by id" action for interacting with a generic persistance medium
/// </summary>
/// <remarks>
/// Creation Date: 10th of March, 2022
/// </remarks>
/// <typeparam name="TEntity">The type used as a result for the "get by id" action. It should implement <see cref="IStorageEntity"/></typeparam>
public interface IGetByIdRepositoryAction<TEntity> where TEntity : IStorageEntity
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets an entity of type <typeparamref name="TEntity"/> identified by <paramref name="id"/> from the storage medium
    /// </summary>
    /// <param name="id">The id of the entity to get</param>
    /// <returns>An entity of type <typeparamref name="TEntity"/> identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{TEntity}"/></returns>
    Task<ApiResponse<TEntity>> GetByIdAsync(string id);
    #endregion
}