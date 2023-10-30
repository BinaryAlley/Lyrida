#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.Repositories.Common.Base;
using Lyrida.DataAccess.Repositories.Common.Actions;
using Lyrida.DataAccess.Common.DTO.Authorization;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.UserRoles;

/// <summary>
/// User role repository interface for the bridge-through between the generic storage medium and storage medium for UserRoles
/// </summary>
/// <remarks>
/// Creation Date: 11th of July, 2023
/// </remarks>
public interface IUserRoleRepository : IRepository<UserRoleDto>,
                                       IInsertRepositoryAction<UserRoleDto>,
                                       IUpdateRepositoryAction<UserRoleDto>
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the role of a user identified by <paramref name="id"/> from the storage medium
    /// </summary>
    /// <param name="id">The Id of the user whose role to get</param>
    /// <returns>A role of a user identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{UserRoleDto}"/></returns>
    Task<ApiResponse<UserRoleDto>> GetByUserIdAsync(string id);
    #endregion
}