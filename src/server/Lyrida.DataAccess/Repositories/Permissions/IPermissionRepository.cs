#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.Repositories.Common.Base;
using Lyrida.DataAccess.Repositories.Common.Actions;
using Lyrida.DataAccess.Common.DTO.Authorization;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Permissions;

/// <summary>
/// Permission repository interface for the bridge-through between the generic storage medium and storage medium for Permissions
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public interface IPermissionRepository : IRepository<PermissionDto>,
                                         IInsertRepositoryAction<PermissionDto>,
                                         IGetAllRepositoryAction<PermissionDto>
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the permissions of the user identified by <paramref name="userId"/> from the storage medium
    /// </summary>
    /// <param name="userId">The id of the user whose permissions to get</param>
    /// <returns>A list of permissions of a user identified by <paramref name="userId"/>, wrapped in a generic API container of type <see cref="ApiResponse{PermissionDto}"/></returns>
    Task<ApiResponse<PermissionDto>> GetAllByUserIdAsync(int userId);
    #endregion
}