#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Lyrida.DataAccess.Common.Enums;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.DTO.Pages;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.UserPages;

/// <summary>
/// User pages repository for the bridge-through between the generic storage medium and storage medium for UserPages
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
internal sealed class UserPageRepository : IUserPageRepository
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDataAccess dataAccess;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="dataAccess">Injected data access service.</param>
    public UserPageRepository(IDataAccess dataAccess)
    {
        this.dataAccess = dataAccess ?? throw new ArgumentException("Data access cannot be null!");
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Opens a transaction.
    /// </summary>
    public void OpenTransaction()
    {
        dataAccess.OpenTransaction();
    }

    /// <summary>
    /// Closes a transaction, rolls back changes if the transaction was faulty.
    /// </summary>
    public void CloseTransaction()
    {
        dataAccess.CloseTransaction();
    }

    /// <summary>
    /// Gets the user page identified by <paramref name="pageId"/> from the storage medium.
    /// </summary>
    /// <param name="pageId">The Id of the user page to get.</param>
    /// <returns>The user page identified by <paramref name="pageId"/>, wrapped in a generic API container of type <see cref="ApiResponse{PageDto}"/>.</returns>
    public async Task<ApiResponse<PageDto>> GetByIdAsync(string pageId)
    {
        return await dataAccess.SelectAsync<PageDto>(DataContainers.UserPages, new { page_id = pageId });
    }

    /// <summary>
    /// Gets the user pages of the user identified by <paramref name="userId"/> from the storage medium.
    /// </summary>
    /// <param name="userId">The Id of the user whose user pages to get.</param>
    /// <returns>The user pages of a user identified by <paramref name="userId"/>, wrapped in a generic API container of type <see cref="ApiResponse{PageDto}"/>.</returns>
    public async Task<ApiResponse<PageDto>> GetByUserIdAsync(string userId)
    {
        return await dataAccess.SelectAsync<PageDto>(DataContainers.UserPages, new { user_id = userId });
    }

    /// <summary>
    /// Saves a user page into the storage medium.
    /// </summary>
    /// <param name="data">The user page to be saved.</param>
    /// <returns>The result of saving <paramref name="data"/>, wrapped in a generic API container of type <see cref="ApiResponse{PageDto}"/>.</returns>
    public async Task<ApiResponse<PageDto>> InsertAsync(PageDto data)
    {
        return await dataAccess.InsertAsync(DataContainers.UserPages, data);
    }

    /// <summary>
    /// Deletes a user page identified by <paramref name="pageId"/> from the storage medium.
    /// </summary>
    /// <param name="pageId">The id of the user page to be deleted.</param>
    /// <returns>The result of deleting the user page, wrapped in a generic API container of type <see cref="ApiResponse"/>.</returns>
    public async Task<ApiResponse> DeleteByIdAsync(string pageId)
    {
        return await dataAccess.DeleteAsync(DataContainers.UserPages, new { page_id = pageId });
    }

    /// <summary>
    /// Updates <paramref name="data"/> in the storage medium.
    /// </summary>
    /// <param name="data">The element that will be updated.</param>
    /// <returns>The result of updating <paramref name="data"/>, wrapped in a generic API container of type <see cref="ApiResponse"/>.</returns>
    public async Task<ApiResponse> UpdateAsync(PageDto data)
    {
        return await dataAccess.UpdateAsync(DataContainers.UserPages, data, new { page_id = data.PageId });
    }
    #endregion
}