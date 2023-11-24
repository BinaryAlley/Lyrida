#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Lyrida.DataAccess.Common.Enums;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.DTO.Common;
using Lyrida.DataAccess.Common.DTO.Configuration;
#endregion

namespace Lyrida.DataAccess.Repositories.Configuration;

/// <summary>
/// User preferences repository for the bridge-through between the generic storage medium and storage medium for UserPreferences
/// </summary>
/// <remarks>
/// Creation Date: 25th of October, 2023
/// </remarks>
internal sealed class UserPreferenceRepository : IUserPreferenceRepository
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDataAccess dataAccess;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="dataAccess">Injected data access service</param>
    public UserPreferenceRepository(IDataAccess dataAccess)
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
    /// Gets the preferences of the user identified by <paramref name="id"/> from the storage medium.
    /// </summary>
    /// <param name="id">The Id of the user whose preferences to get.</param>
    /// <returns>The preferences of a user identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{ProfilePreferencesDto}"/>.</returns>
    public async Task<ApiResponse<ProfilePreferencesDto>> GetByIdAsync(string id)
    {
        return await dataAccess.SelectAsync<ProfilePreferencesDto>(DataContainers.UserPreferences, new { user_id = id });
    }

    /// <summary>
    /// Adds the profile preferences to a user identified by <paramref name="userId"/>, in the storage medium.
    /// </summary>
    /// <param name="userId">The id of the user whose preferences are added.</param>
    /// <param name="data">The profile preferences to be added.</param>
    /// <returns>The result of saving <paramref name="data"/>, wrapped in a generic API container of type <see cref="ApiResponse{ProfilePreferencesDto}"/>.</returns>
    public async Task<ApiResponse<ProfilePreferencesDto>> InsertAsync(ProfilePreferencesDto data)
    {
        return await dataAccess.InsertAsync(DataContainers.UserPreferences, data);
    }

    /// <summary>
    /// Updates <paramref name="data"/> in the storage medium.
    /// </summary>
    /// <param name="data">The element that will be updated.</param>
    /// <returns>The result of updating <paramref name="data"/>, wrapped in a generic API container of type <see cref="ApiResponse"/>.</returns>
    public async Task<ApiResponse> UpdateAsync(ProfilePreferencesDto data)
    {
        return await dataAccess.UpdateAsync(DataContainers.UserPreferences, data, new { user_id = data.UserId });
    }
    #endregion
}