#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Common.Actions;

/// <summary>
/// Interface defining the "get all" action for interacting with a generic persistance medium
/// </summary>
/// <remarks>
/// Creation Date: 10th of March, 2022
/// </remarks>
/// <typeparam name="TDto">The type used as a result for the "get all" action. It should implement <see cref="IStorageDto"/></typeparam>
public interface IGetAllRepositoryAction<TDto> where TDto : IStorageDto
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets all data of type <typeparamref name="TDto"/> from the storage medium
    /// </summary>
    /// <returns>A list of <typeparamref name="TDto"/>, wrapped in a generic API container of type <see cref="ApiResponse{TDto}"/></returns>
    Task<ApiResponse<TDto>> GetAllAsync();
    #endregion
}