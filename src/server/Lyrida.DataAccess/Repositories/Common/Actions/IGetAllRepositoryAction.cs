#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Entities.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Common.Actions;

/// <summary>
/// Interface defining the "get all" action for interacting with a generic persistance medium
/// </summary>
/// <remarks>
/// Creation Date: 10th of March, 2022
/// </remarks>
/// <typeparam name="TEntity">The type used as a result for the "get all" action. It should implement <see cref="IStorageEntity"/></typeparam>
public interface IGetAllRepositoryAction<TEntity> where TEntity : IStorageEntity
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets all data of type <typeparamref name="TEntity"/> from the storage medium
    /// </summary>
    /// <returns>A list of <typeparamref name="TEntity"/>, wrapped in a generic API container of type <see cref="ApiResponse{TEntity}"/></returns>
    Task<ApiResponse<TEntity>> GetAllAsync();
    #endregion
}