#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Common.Actions;

/// <summary>
/// Interface defining the "insert" action for interacting with a generic persistance medium
/// </summary>
/// <remarks>
/// Creation Date: 10th of March, 2022
/// </remarks>
/// <typeparam name="TDto">The type used for the insert action. It should implement <see cref="IStorageDto"/></typeparam>
public interface IInsertRepositoryAction<TDto> where TDto : IStorageDto
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Saves an element of type <typeparamref name="TDto"/> in the storage medium
    /// </summary>
    /// <param name="data">The element to be saved</param>
    /// <returns>The result of saving <paramref name="data"/>, wrapped in a generic API container of type <see cref="ApiResponse{TDto}"/></returns>
    Task<ApiResponse<TDto>> InsertAsync(TDto data);
    #endregion
}