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
                                       IUpdateRepositoryAction<PageDto>,
                                       IGetByIdRepositoryAction<PageDto>,
                                       IDeleteByIdRepositoryAction
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the user pages of the user identified by <paramref name="userId"/> from the storage medium.
    /// </summary>
    /// <param name="userId">The Id of the user whose user pages to get.</param>
    /// <returns>The user pages of a user identified by <paramref name="userId"/>, wrapped in a generic API container of type <see cref="ApiResponse{PageDto}"/>.</returns>
    Task<ApiResponse<PageDto>> GetByUserIdAsync(string userId);
    #endregion
}