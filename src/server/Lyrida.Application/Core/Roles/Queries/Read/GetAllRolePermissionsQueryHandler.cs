#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using ErrorOr;
using System.Linq;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Common.Errors;
using Lyrida.Application.Core.Authorization;
using Lyrida.Application.Common.DTO.Authorization;
using Lyrida.DataAccess.Repositories.RolePermissions;
#endregion

namespace Lyrida.Application.Core.Roles.Queries.Read;

/// <summary>
/// Get role permissions query handler
/// </summary>
/// <remarks>
/// Creation Date: 11th of August, 2023
/// </remarks>
public class GetAllRolePermissionsQueryHandler : IRequestHandler<GetAllRolePermissionsQuery, ErrorOr<IEnumerable<PermissionDto>>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IAuthorizationService authorizationService;
    private readonly IRolePermissionRepository rolePermissionRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="authorizationService">Injected service for permissions</param>
    public GetAllRolePermissionsQueryHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
    {
        rolePermissionRepository = unitOfWork.GetRepository<IRolePermissionRepository>();
        this.authorizationService = authorizationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the list of role permissions stored in the repository
    /// </summary>
    /// <returns>A list of role permissions</returns>
    public async Task<ErrorOr<IEnumerable<PermissionDto>>> Handle(GetAllRolePermissionsQuery request, CancellationToken cancellationToken)
    {
        // check if the user has the permission to perform the action
        if (authorizationService.UserPermissions.CanViewPermissions)
        {
            // get the list of role permissions from the repository
            var resultSelectRolePermissions = await rolePermissionRepository.GetByIdAsync(request.RoleId.ToString());
            if (resultSelectRolePermissions.Error is null)
            {
                if (resultSelectRolePermissions.Data is not null) // repository returns role permissions, convert to permissions
                    return resultSelectRolePermissions.Data.Select(rolePermission => new PermissionDto() 
                    {
                        Id = rolePermission.PermissionId, 
                        PermissionName = rolePermission.PermissionName 
                    }).ToArray();
                else
                    return Array.Empty<PermissionDto>();
            }
            else
                return Errors.DataAccess.GetRolePermissionsError;
        }
        else
            return Errors.Authorization.InvalidPermissionError;
    }
    #endregion
}