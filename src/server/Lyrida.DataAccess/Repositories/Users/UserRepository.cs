#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Lyrida.DataAccess.Common.Enums;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.DTO.Authentication;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Users;

/// <summary>
/// User repository for the bridge-through between the generic storage medium and storage medium for Users
/// </summary>
/// <remarks>
/// Creation Date: 12th of July, 2023
/// </remarks>
internal sealed class UserRepository : IUserRepository
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDataAccess dataAccess;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="dataAccess">Injected data access service</param>
    public UserRepository(IDataAccess dataAccess)
    {
        this.dataAccess = dataAccess ?? throw new ArgumentException("Data access cannot be null!");
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Opens a transaction
    /// </summary>
    public void OpenTransaction()
    {
        dataAccess.OpenTransaction();
    }

    /// <summary>
    /// Closes a transaction, rolls back changes if the transaction was faulty
    /// </summary>
    public void CloseTransaction()
    {
        dataAccess.CloseTransaction();
    }

    /// <summary>
    /// Updates the password for <paramref name="data"/> in the storage medium
    /// </summary>
    /// <param name="data">The user whose password will be updated</param>
    /// <returns>The result of updating the password of <paramref name="data"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
    public async Task<ApiResponse> ChangePasswordAsync(UserDto data)
    {
        return await dataAccess.UpdateAsync(DataContainers.Users, new { data.Password }, new { data.Email });
    }

    /// <summary>
    /// Deletes a user identified by <paramref name="id"/> from the storage medium
    /// </summary>
    /// <param name="id">The id of the user to be deleted</param>
    /// <returns>The result of deleting the user, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
    public async Task<ApiResponse> DeleteByIdAsync(string id)
    {
        OpenTransaction();
        // delete not just the user, but its role(s) and permission(s)
        ApiResponse response = await dataAccess.DeleteAsync(DataContainers.Users, new { id });
        response.Error = (await dataAccess.DeleteAsync(DataContainers.UserRoles, new { user_id = id }))?.Error ?? response.Error;
        response.Error = (await dataAccess.DeleteAsync(DataContainers.UserPermissions, new { user_id = id }))?.Error ?? response.Error;
        CloseTransaction();
        return response;
    }

    /// <summary>
    /// Gets all users from the storage medium
    /// </summary>
    /// <returns>A list of users, wrapped in a generic API container of type <see cref="ApiResponse{UserDto}"/></returns>
    public async Task<ApiResponse<UserDto>> GetAllAsync()
    {
        return await dataAccess.SelectAsync<UserDto>(DataContainers.Users);
    }

    /// <summary>
    /// Gets the user identified by <paramref name="id"/> from the storage medium
    /// </summary>
    /// <param name="id">The Id of the user to get</param>
    /// <returns>A user identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{UserDto}"/></returns>
    public async Task<ApiResponse<UserDto>> GetByIdAsync(string id)
    {
        return await dataAccess.SelectAsync<UserDto>(DataContainers.Users, new { id });
    }

    /// <summary>
    /// Gets the user identified by <paramref name="email"/> from the storage medium
    /// </summary>
    /// <param name="email">The email of the user to get</param>
    /// <returns>A user identified by <paramref name="email"/>, wrapped in a generic API container of type <see cref="ApiResponse{UserDto}"/></returns>
    public async Task<ApiResponse<UserDto>> GetByEmailAsync(string email)
    {
        return await dataAccess.SelectAsync<UserDto>(DataContainers.Users, new { email });
    }

    /// <summary>
    /// Gets the user with the <paramref name="token"/> registration token from the storage medium
    /// </summary>
    /// <param name="token">The registration validation token of the user to get</param>
    /// <returns>A user with the <paramref name="token"/> registration token, wrapped in a generic API container of type <see cref="ApiResponse{UserDto}"/></returns>
    public async Task<ApiResponse<UserDto>> GetByValidationTokenAsync(string token)
    {
        return await dataAccess.SelectAsync<UserDto>(DataContainers.Users, new { verification_token = token });
    }

    /// <summary>
    /// Saves a user in the storage medium
    /// </summary>
    /// <param name="data">The user to be saved</param>
    /// <returns>The result of saving <paramref name="data"/>, wrapped in a generic API container of type <see cref="ApiResponse{UserDto}"/></returns>
    public async Task<ApiResponse<UserDto>> InsertAsync(UserDto data)
    {
        return await dataAccess.InsertAsync(DataContainers.Users, data);
    }

    /// <summary>
    /// Updates <paramref name="data"/> in the storage medium
    /// </summary>
    /// <param name="data">The element that will be updated</param>
    /// <returns>The result of updating <paramref name="data"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
    public async Task<ApiResponse> UpdateAsync(UserDto data)
    {
        return await dataAccess.UpdateAsync(DataContainers.Users, data, new { id = data.Id });
    }
    #endregion
}