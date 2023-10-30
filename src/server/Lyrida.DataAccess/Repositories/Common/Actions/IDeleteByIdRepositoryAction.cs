#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Common.Actions;

/// <summary>
/// Interface defining the "delete by id" action for interacting with a generic persistance medium
/// </summary>
/// <remarks>
/// Creation Date: 10th of March, 2022
/// </remarks>
public interface IDeleteByIdRepositoryAction
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Deletes an element identified by <paramref name="id"/> from the storage medium
    /// </summary>
    /// <param name="id">The id of the element to be deleted</param>
    /// <returns>The result of deleting the element, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
    Task<ApiResponse> DeleteByIdAsync(string id);
    #endregion
}