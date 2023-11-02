#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.Common.DTO.Pages;
using Lyrida.DataAccess.Common.DTO.Common;
using Lyrida.DataAccess.Repositories.Common.Base;
using Lyrida.DataAccess.Repositories.Common.Actions;
#endregion

namespace Lyrida.DataAccess.Repositories.UserPages;

/// <summary>
/// User pages repository interface for the bridge-through between the generic storage medium and storage medium for UserPages
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
public interface IUserPageRepository : IRepository<PageDto>,
                                       IInsertRepositoryAction<PageDto>, 
                                       IGetByIdRepositoryAction<PageDto>
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Deletes a user page identified by <paramref name="userId"/> and <paramref name="pageId"/> from the storage medium.
    /// </summary>
    /// <param name="userId">The id of the user whose user page is deleted.</param>
    /// <param name="userId">The id of the user page to be deleted.</param>
    /// <returns>The result of deleting the user page, wrapped in a generic API container of type <see cref="ApiResponse"/>.</returns>
    Task<ApiResponse> DeleteByIdAsync(string userId, string pageId);

    /// <summary>
    /// Updates <paramref name="data"/> in the storage medium.
    /// </summary>
    /// <param name="data">The element that will be updated.</param>
    /// <returns>The result of updating <paramref name="data"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
    Task<ApiResponse> UpdateAsync(PageDto data);
    #endregion
}