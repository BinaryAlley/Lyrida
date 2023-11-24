#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Common.Actions;

/// <summary>
/// Interface defining the "update" action for interacting with a generic persistance medium
/// </summary>
/// <remarks>
/// Creation Date: 10th of March, 2022
/// </remarks>
/// <typeparam name="TDto">The type used for the update action. It should implement <see cref="IStorageDto"/></typeparam>
public interface IUpdateRepositoryAction<TDto> where TDto : IStorageDto
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Updates <paramref name="data"/> in the storage medium.
    /// </summary>
    /// <param name="data">The element that will be updated.</param>
    /// <returns>The result of updating the <paramref name="data"/>, wrapped in a generic API container of type <see cref="ApiResponse"/>.</returns>
    Task<ApiResponse> UpdateAsync(TDto data);
    #endregion
}