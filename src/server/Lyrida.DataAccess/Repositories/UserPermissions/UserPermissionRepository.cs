#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.DataAccess.Common.Enums;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Entities.Common;
using Lyrida.DataAccess.Common.Entities.Authorization;
#endregion

namespace Lyrida.DataAccess.Repositories.UserPermissions;

/// <summary>
/// User permission repository for the bridge-through between the generic storage medium and storage medium for UserPermissions
/// </summary>
/// <remarks>
/// Creation Date: 18th of July, 2023
/// </remarks>
internal sealed class UserPermissionRepository : IUserPermissionRepository
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDataAccess dataAccess;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="dataAccess">Injected data access service</param>
    public UserPermissionRepository(IDataAccess dataAccess)
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
    /// Gets the permissions of the user identified by <paramref name="id"/> from the storage medium
    /// </summary>
    /// <param name="id">The Id of the user whose permissions to get</param>
    /// <returns>The permssions of a user identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{UserPermissionEntity}"/></returns>
    public async Task<ApiResponse<UserPermissionEntity>> GetByIdAsync(string id)
    {
        return await dataAccess.ExecuteAsync<UserPermissionEntity>("SELECT up.id, up.user_id AS UserId, up.permission_id AS PermissionId, p.permission_name AS PermissionName " +
            "FROM Users AS u " +
            "JOIN UserPermissions AS up ON u.id = up.user_id " +
            "JOIN Permissions AS p ON up.permission_id = p.id " +
            "WHERE u.id = @id", new { id });
    }

    /// <summary>
    /// Adds the <paramref name="permissions"/> list to a user identified by <paramref name="userId"/> in the storage medium
    /// </summary>
    /// <param name="userId">The id of the user whose permissions are added</param>
    /// <param name="permissions">The list of permissions of the role that is created</param>
    /// <returns>A role identified by <paramref name="name"/>, wrapped in a generic API container of type <see cref="ApiResponse{RoleEntity}"/></returns>
    public async Task<ApiResponse> InsertAsync(string userId, List<int> permissions)
    {
        // check if the user is the admin account
        var resultAdmin = await dataAccess.ExecuteAsync("SELECT CASE WHEN COUNT(*) > 0 THEN u.id ELSE NULL END AS UserId " +
            "FROM Users AS u " +
            "JOIN UserRoles AS ur ON u.id = ur.user_id " +
            "JOIN Roles AS r ON ur.role_id = r.id " +
            "WHERE u.id = @userId AND r.role_name = 'Admin' " +
            "GROUP BY u.id;", new { userId });
        if (resultAdmin.Count > 0)
            return new ApiResponse() { Error = "Cannot update admin permissions!" };
        else
        {
            OpenTransaction();
            // delete all the user permissions
            ApiResponse response = await dataAccess.DeleteAsync(EntityContainers.UserPermissions, new { user_id = userId });
            // add the permissions of the role
            foreach (int permissionId in permissions)
                response.Error = (await dataAccess.ExecuteAsync("INSERT INTO UserPermissions (user_id, permission_id) VALUES (@user_id, @permission_id)",
                    new { user_id = userId, permission_id = permissionId }))?.Error ?? response.Error;
            // get the newly created role
            CloseTransaction();
            return response;
        }
    }
    #endregion
}