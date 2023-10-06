#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.DataAccess.Common.Entities.Common;
using Lyrida.DataAccess.Repositories.Common.Base;
using Lyrida.DataAccess.Repositories.Common.Actions;
using Lyrida.DataAccess.Common.Entities.Authorization;
#endregion

namespace Lyrida.DataAccess.Repositories.Roles;

/// <summary>
/// Role repository interface for the bridge-through between the generic storage medium and storage medium for Roles
/// </summary>
/// <remarks>
/// Creation Date: 09th of July, 2023
/// </remarks>
public interface IRoleRepository : IRepository<RoleEntity>,
                                   IDeleteByIdRepositoryAction,
                                   IGetAllRepositoryAction<RoleEntity>,
                                   IGetByIdRepositoryAction<RoleEntity>
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the role identified by <paramref name="name"/> from the storage medium
    /// </summary>
    /// <param name="name">The name of the role to get</param>
    /// <returns>A role identified by <paramref name="name"/>, wrapped in a generic API container of type <see cref="ApiResponse{RoleEntity}"/></returns>
    Task<ApiResponse<RoleEntity>> GetByNameAsync(string name);

    /// <summary>
    /// Creates a role identified by <paramref name="name"/> and the <paramref name="permissions"/> list of permissions in the storage medium
    /// </summary>
    /// <param name="name">The name of the role to create</param>
    /// <param name="permissions">The list of permissions of the role that is created</param>
    /// <returns>A role identified by <paramref name="name"/>, wrapped in a generic API container of type <see cref="ApiResponse{RoleEntity}"/></returns>
    Task<ApiResponse<RoleEntity>> InsertAsync(string name, List<int> permissions);

    /// <summary>
    /// Updates a role identified by <paramref name="roleId"/> and the <paramref name="permissions"/> list of permissions in the storage medium
    /// </summary>
    /// <param name="roleId">The id of the role that will be updated</param>
    /// <param name="name">The new name of the role that will be updated</param>
    /// <returns>The result of updating the role and its permissions, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
    Task<ApiResponse> UpdateAsync(string roleId, string name, List<int> permissions);
    #endregion
}