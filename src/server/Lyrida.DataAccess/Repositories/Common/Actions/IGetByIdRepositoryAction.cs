#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Common.Actions;

/// <summary>
/// Interface defining the "get by id" action for interacting with a generic persistance medium
/// </summary>
/// <remarks>
/// Creation Date: 10th of March, 2022
/// </remarks>
/// <typeparam name="TDto">The type used as a result for the "get by id" action. It should implement <see cref="IStorageDto"/></typeparam>
public interface IGetByIdRepositoryAction<TDto> where TDto : IStorageDto
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets an element of type <typeparamref name="TDto"/> identified by <paramref name="id"/> from the storage medium.
    /// </summary>
    /// <param name="id">The id of the element to get.</param>
    /// <returns>An element of type <typeparamref name="TDto"/> identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{TDto}"/>.</returns>
    Task<ApiResponse<TDto>> GetByIdAsync(string id);
    #endregion
}