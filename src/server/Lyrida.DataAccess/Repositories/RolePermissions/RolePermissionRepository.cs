#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.DTO.Authorization;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.RolePermissions;

/// <summary>
/// Role permission repository for the bridge-through between the generic storage medium and storage medium for RolePermissions
/// </summary>
/// <remarks>
/// Creation Date: 11th of July, 2023
/// </remarks>
internal sealed class RolePermissionRepository : IRolePermissionRepository
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDataAccess dataAccess;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="dataAccess">Injected data access service</param>
    public RolePermissionRepository(IDataAccess dataAccess)
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
    /// Gets the permissions of the role identified by <paramref name="id"/> from the storage medium
    /// </summary>
    /// <param name="id">The Id of the role whose permissions to get</param>
    /// <returns>The permssions of a role identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{RolePermissionDto}"/></returns>
    public async Task<ApiResponse<RolePermissionDto>> GetByIdAsync(string id)
    {
        return await dataAccess.ExecuteAsync<RolePermissionDto>("SELECT rp.id, rp.role_id AS RoleId, rp.permission_id AS PermissionId, p.permission_name AS PermissionName " +
            "FROM RolePermissions As rp " +
            "LEFT JOIN Permissions AS p on rp.permission_id = p.id " +
            "WHERE rp.role_id = @id", new { id });
    }
    #endregion
}