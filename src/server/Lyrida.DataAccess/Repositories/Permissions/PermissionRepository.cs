#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Lyrida.DataAccess.Common.Enums;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.DTO.Authorization;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Permissions;

/// <summary>
/// Permission repository for the bridge-through between the generic storage medium and storage medium for Permissions
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
internal sealed class PermissionRepository : IPermissionRepository
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDataAccess dataAccess;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="dataAccess">Injected data access service</param>
    public PermissionRepository(IDataAccess dataAccess)
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
    /// Gets all permissions from the storage medium
    /// </summary>
    /// <returns>A list of permissions, wrapped in a generic API container of type <see cref="ApiResponse{PermissionDto}"/></returns>
    public async Task<ApiResponse<PermissionDto>> GetAllAsync()
    {
        return await dataAccess.SelectAsync<PermissionDto>(DataContainers.Permissions);
    }

    /// <summary>
    /// Gets the permissions of the user identified by <paramref name="userId"/> from the storage medium
    /// </summary>
    /// <param name="userId">The id of the user whose permissions to get</param>
    /// <returns>A list of permissions of a user identified by <paramref name="userId"/>, wrapped in a generic API container of type <see cref="ApiResponse{PermissionDto}"/></returns>
    public async Task<ApiResponse<PermissionDto>> GetAllByUserIdAsync(int userId)
    {
        return await dataAccess.ExecuteAsync<PermissionDto>("SELECT p.id, p.permission_name AS PermissionName " +
            "FROM (SELECT rp.permission_id " +
            "      FROM UserRoles AS ur " +
            "      JOIN RolePermissions AS rp ON ur.role_id = rp.role_id " +
            "      WHERE ur.user_id = @UserId " +
            "      UNION " +
            "      SELECT up.permission_id " +
            "      FROM UserPermissions up " +
            "      WHERE up.user_id = @UserId) AS user_perm " +
            "JOIN Permissions p ON p.id = user_perm.permission_id;", new { UserId = userId });
    }

    /// <summary>
    /// Saves a permission in the storage medium
    /// </summary>
    /// <param name="data">The permission to be saved</param>
    /// <returns>The result of saving <paramref name="data"/>, wrapped in a generic API container of type <see cref="ApiResponse{PermissionDto}"/></returns>
    public async Task<ApiResponse<PermissionDto>> InsertAsync(PermissionDto data)
    {
        return await dataAccess.InsertAsync(DataContainers.Permissions, data);
    }
    #endregion
}