﻿#region ========================================================================= USING =====================================================================================
using Mapster;
using ErrorOr;
using System.Threading.Tasks;
using Lyrida.DataAccess.UoW;
using Lyrida.Application.Common.Errors.Types;
using Lyrida.DataAccess.Repositories.Permissions;
using Lyrida.Domain.Common.Entities.Authorization;
#endregion

namespace Lyrida.Application.Core.Authorization;

/// <summary>
/// Authorization service
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public class AuthorizationService : IAuthorizationService
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IPermissionRepository permissionRepository;
    #endregion

    #region ==================================================================== PROPERTIES =================================================================================
    public UserPermissions UserPermissions { get; private set; } = null!;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    public AuthorizationService(IUnitOfWork unitOfWork)
    {
        permissionRepository = unitOfWork.GetRepository<IPermissionRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the permissions of a user identified by <paramref name="userId"/>
    /// </summary>
    /// <param name="userId">The id of the user for which to get the permissions</param>
    /// <returns>An entity containing the permissions of a user identified by <paramref name="userId"/></returns>
    public async Task<ErrorOr<bool>> GetUserPermissionsAsync(int userId)
    {
        var resultSelectPermissions = await permissionRepository.GetAllByUserIdAsync(userId);
        if (resultSelectPermissions.Error is null)
        {
            UserPermissions = new UserPermissions(resultSelectPermissions.Data?.Adapt<PermissionEntity[]>());
            return true;
        }
        else
            return Errors.DataAccess.GetUserPermissionsError;
    }
    #endregion
}