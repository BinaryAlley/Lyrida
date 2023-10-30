#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.DataAccess.Common.Enums;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.DTO.Authorization;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Roles;

/// <summary>
/// Role repository for the bridge-through between the generic storage medium and storage medium for Roles
/// </summary>
/// <remarks>
/// Creation Date: 09th of July, 2023
/// </remarks>
internal sealed class RoleRepository : IRoleRepository
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDataAccess dataAccess;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="dataAccess">Injected data access service</param>
    public RoleRepository(IDataAccess dataAccess)
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
    /// Gets all roles from the storage medium
    /// </summary>
    /// <returns>A list of roles, wrapped in a generic API container of type <see cref="ApiResponse{RoleDto}"/></returns>
    public async Task<ApiResponse<RoleDto>> GetAllAsync()
    {
        return await dataAccess.SelectAsync<RoleDto>(DataContainers.Roles);
    }

    /// <summary>
    /// Gets the role identified by <paramref name="id"/> from the storage medium
    /// </summary>
    /// <param name="id">The Id of the role to get</param>
    /// <returns>A role identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{RoleDto}"/></returns>
    public async Task<ApiResponse<RoleDto>> GetByIdAsync(string id)
    {
        return await dataAccess.SelectAsync<RoleDto>(DataContainers.Roles, new { id });
    }

    /// <summary>
    /// Gets the role identified by <paramref name="name"/> from the storage medium
    /// </summary>
    /// <param name="name">The name of the role to get</param>
    /// <returns>A role identified by <paramref name="name"/>, wrapped in a generic API container of type <see cref="ApiResponse{RoleDto}"/></returns>
    public async Task<ApiResponse<RoleDto>> GetByNameAsync(string name)
    {
        return await dataAccess.SelectAsync<RoleDto>(DataContainers.Roles, new { role_name = name });
    }

    /// <summary>
    /// Creates a role identified by <paramref name="name"/> and the <paramref name="permissions"/> list of permissions in the storage medium
    /// </summary>
    /// <param name="name">The name of the role to create</param>
    /// <param name="permissions">The list of permissions of the role that is created</param>
    /// <returns>A role identified by <paramref name="name"/>, wrapped in a generic API container of type <see cref="ApiResponse{RoleDto}"/></returns>
    public async Task<ApiResponse<RoleDto>> InsertAsync(string name, List<int> permissions)
    {
        OpenTransaction();
        // add the role
        ApiResponse<RoleDto> response = await dataAccess.InsertAsync(DataContainers.Roles, new RoleDto() { RoleName = name });
        // add the permissions of the role
        foreach (int permissionId in permissions)
            response.Error = (await dataAccess.ExecuteAsync("INSERT INTO RolePermissions (role_id, permission_id) VALUES (@role_id, @permission_id)", 
                new { role_id = response.Data![0].Id, permission_id = permissionId }))?.Error ?? response.Error;
        // get the newly created role
        response = await dataAccess.SelectAsync<RoleDto>(DataContainers.Roles, new { id = response.Data![0].Id });
        CloseTransaction();
        return response;
    }

    /// <summary>
    /// Deletes a role identified by <paramref name="id"/> from the storage medium
    /// </summary>
    /// <param name="id">The id of the role to be deleted</param>
    /// <returns>The result of deleting the role, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
    public async Task<ApiResponse> DeleteByIdAsync(string id)
    {
        OpenTransaction();
        // delete the role, the user roles and the permission roles associated with it
        ApiResponse response = await dataAccess.DeleteAsync(DataContainers.Roles, new { id });
        response.Error = (await dataAccess.DeleteAsync(DataContainers.UserRoles, new { role_id = id }))?.Error ?? response.Error;
        response.Error = (await dataAccess.DeleteAsync(DataContainers.RolePermissions, new { role_id = id }))?.Error ?? response.Error;
        CloseTransaction();
        return response;
    }

    /// <summary>
    /// Updates a role identified by <paramref name="roleId"/> and the <paramref name="permissions"/> list of permissions in the storage medium
    /// </summary>
    /// <param name="roleId">The id of the role that will be updated</param>
    /// <param name="name">The new name of the role that will be updated</param>
    /// <returns>The result of updating the role and its permissions, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
    public async Task<ApiResponse> UpdateAsync(string roleId, string name, List<int> permissions)
    {
        OpenTransaction();
        // update the role
        ApiResponse response = await dataAccess.UpdateAsync(DataContainers.Roles, new { role_name = name }, new { id = roleId });
        // delete the permissions of the role
        response.Error = (await dataAccess.DeleteAsync(DataContainers.RolePermissions, new { role_id = roleId }))?.Error ?? response.Error;
        // add the permissions of the role
        foreach (int permissionId in permissions)
            response.Error = (await dataAccess.ExecuteAsync("INSERT INTO RolePermissions (role_id, permission_id) VALUES (@role_id, @permission_id)",
                new { role_id = roleId, permission_id = permissionId }))?.Error ?? response.Error;
        CloseTransaction();
        return response;
    }
    #endregion
}