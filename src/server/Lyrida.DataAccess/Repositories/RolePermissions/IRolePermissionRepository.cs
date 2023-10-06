#region ========================================================================= USING =====================================================================================
using Lyrida.DataAccess.Repositories.Common.Base;
using Lyrida.DataAccess.Repositories.Common.Actions;
using Lyrida.DataAccess.Common.Entities.Authorization;
#endregion

namespace Lyrida.DataAccess.Repositories.RolePermissions;

/// <summary>
/// Role permission repository interface for the bridge-through between the generic storage medium and storage medium for RolePermissions
/// </summary>
/// <remarks>
/// Creation Date: 11th of July, 2023
/// </remarks>
public interface IRolePermissionRepository : IRepository<RolePermissionEntity>,
                                             IGetByIdRepositoryAction<RolePermissionEntity>
{
}