#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using Mapster;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.DataAccess.UoW;
using System.Collections.Generic;
using Lyrida.Application.Core.Authorization;
using Lyrida.Application.Common.Errors.Types;
using Lyrida.DataAccess.Repositories.Permissions;
using Lyrida.Domain.Common.Entities.Authorization;
#endregion

namespace Lyrida.Application.Core.Permissions.Queries.Read;

/// <summary>
/// Get permissions query handler
/// </summary>
/// <remarks>
/// Creation Date: 11th of August, 2023
/// </remarks>
public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, ErrorOr<IEnumerable<PermissionEntity>>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IPermissionRepository permissionRepository;
    private readonly IAuthorizationService authorizationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="authorizationService">Injected service for permissions</param>
    public GetAllPermissionsQueryHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
    {
        permissionRepository = unitOfWork.GetRepository<IPermissionRepository>();
        this.authorizationService = authorizationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the list of permissions stored in the repository
    /// </summary>
    /// <returns>A list of permissions</returns>
    public async Task<ErrorOr<IEnumerable<PermissionEntity>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        // check if the user has the permission to perform the action
        if (authorizationService.UserPermissions.CanViewPermissions)
        {
            // get the list of permissions from the repository
            var resultSelectPermissions = await permissionRepository.GetAllAsync();
            if (resultSelectPermissions.Error is null)
            {
                if (resultSelectPermissions.Data is not null)
                    return resultSelectPermissions.Data.Adapt<PermissionEntity[]>();
                else
                    return Array.Empty<PermissionEntity>();
            }
            else
                return Errors.DataAccess.GetUserPermissionsError;
        }
        else
            return Errors.Authorization.InvalidPermission;
    }
    #endregion
}