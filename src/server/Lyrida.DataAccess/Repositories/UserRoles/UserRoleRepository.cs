#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Lyrida.DataAccess.Common.Enums;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Entities.Common;
using Lyrida.DataAccess.Common.Entities.Authorization;
#endregion

namespace Lyrida.DataAccess.Repositories.UserRoles;

/// <summary>
/// User role repository for the bridge-through between the generic storage medium and storage medium for UserRoles
/// </summary>
/// <remarks>
/// Creation Date: 11th of July, 2023
/// </remarks>
internal sealed class UserRoleRepository : IUserRoleRepository
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDataAccess dataAccess;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="dataAccess">Injected data access service</param>
    public UserRoleRepository(IDataAccess dataAccess)
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
    /// Gets the role of a user identified by <paramref name="id"/> from the storage medium
    /// </summary>
    /// <param name="id">The Id of the user whose role to get</param>
    /// <returns>A role of a user identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{UserRoleEntity}"/></returns>
    public async Task<ApiResponse<UserRoleEntity>> GetByUserIdAsync(string id)
    {
        return await dataAccess.SelectAsync<UserRoleEntity>(EntityContainers.UserRoles, new { user_id = id });
    }

    /// <summary>
    /// Saves a user role in the storage medium
    /// </summary>
    /// <param name="entity">The user role to be saved</param>
    /// <returns>The result of saving <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse{UserRoleEntity}"/></returns>
    public async Task<ApiResponse<UserRoleEntity>> InsertAsync(UserRoleEntity entity)
    {
        return await dataAccess.InsertAsync(EntityContainers.UserRoles, entity);
    }

    /// <summary>
    /// Updates <paramref name="entity"/> in the storage medium
    /// </summary>
    /// <param name="entity">The entity that will be updated</param>
    /// <returns>The result of updating <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
    public async Task<ApiResponse> UpdateAsync(UserRoleEntity entity)
    {
        OpenTransaction();
        // check if the user is the admin account
        var resultIsAdminAccount = await dataAccess.ExecuteAsync("SELECT CASE WHEN COUNT(*) > 0 THEN u.id ELSE NULL END AS UserId " +
            "FROM Users AS u " +
            "JOIN UserRoles AS ur ON u.id = ur.user_id " +
            "JOIN Roles AS r ON ur.role_id = r.id " +
            "WHERE u.id = @userId AND r.role_name = 'Admin' " +
            "GROUP BY u.id;", new { userId = entity.UserId });
        if (resultIsAdminAccount.Count > 0)
            return new ApiResponse() { Error = "Cannot set admin role!" };
        var resultIsAdminRole = await dataAccess.ExecuteAsync("SELECT CASE WHEN COUNT(*) > 0 THEN r.id ELSE NULL END AS RoleId " +
            "FROM Roles AS r " +
            "WHERE r.id = @roleId AND r.role_name = 'Admin' " +
            "GROUP BY r.id;", new { roleId = entity.RoleId });
        if (resultIsAdminRole.Count > 0)
            return new ApiResponse() { Error = "Cannot set admin role!" };
        // delete all roles of the user
        ApiResponse response = await dataAccess.DeleteAsync(EntityContainers.UserRoles, new { user_id = entity.UserId });
        // add the role of the user
        if (entity.RoleId > 0) // id 0 means "no role"
            response.Error = (await dataAccess.InsertAsync(EntityContainers.UserRoles, entity))?.Error ?? response.Error;
        CloseTransaction();
        return response;
    }
    #endregion
}