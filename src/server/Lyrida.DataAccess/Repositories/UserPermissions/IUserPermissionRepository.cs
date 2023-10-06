#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.DataAccess.Common.Entities.Common;
using Lyrida.DataAccess.Repositories.Common.Base;
using Lyrida.DataAccess.Repositories.Common.Actions;
using Lyrida.DataAccess.Common.Entities.Authorization;
#endregion

namespace Lyrida.DataAccess.Repositories.UserPermissions;

/// <summary>
/// User permission repository interface for the bridge-through between the generic storage medium and storage medium for UserPermissions
/// </summary>
/// <remarks>
/// Creation Date: 18th of July, 2023
/// </remarks>
public interface IUserPermissionRepository : IRepository<UserPermissionEntity>,
                                             IGetByIdRepositoryAction<UserPermissionEntity>
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Adds the <paramref name="permissions"/> list to a user identified by <paramref name="userId"/> in the storage medium
    /// </summary>
    /// <param name="userId">The id of the user whose permissions are added</param>
    /// <param name="permissions">The list of permissions of the role that is created</param>
    /// <returns>A role identified by <paramref name="name"/>, wrapped in a generic API container of type <see cref="ApiResponse{RoleEntity}"/></returns>
    Task<ApiResponse> InsertAsync(string userId, List<int> permissions);
    #endregion
}