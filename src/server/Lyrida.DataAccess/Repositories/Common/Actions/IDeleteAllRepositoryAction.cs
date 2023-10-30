#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Common.Actions;

/// <summary>
/// Interface defining the "delete all" action for interacting with a generic persistance medium
/// </summary>
/// <remarks>
/// Creation Date: 10th of March, 2022
/// </remarks>
public interface IDeleteAllRepositoryAction
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Deletes all entities from the storage medium
    /// </summary>
    /// <returns>The result of deleting the entities, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
    Task<ApiResponse> DeleteAllAsync();
    #endregion
}