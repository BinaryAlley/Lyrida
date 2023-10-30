#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using Mapster;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Common.Errors;
using Lyrida.Application.Core.Authorization;
using Lyrida.Application.Common.DTO.Authorization;
using Lyrida.DataAccess.Repositories.UserPermissions;
#endregion

namespace Lyrida.Application.Core.Users.Queries.Read;

/// <summary>
/// Get user permissions query handler
/// </summary>
/// <remarks>
/// Creation Date: 18th of August, 2023
/// </remarks>
public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, ErrorOr<IEnumerable<UserPermissionDto>>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserPermissionRepository userPermissionRepository;
    private readonly IAuthorizationService authorizationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="authorizationService">Injected service for permissions</param>
    public GetUserPermissionsQueryHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
    {
        userPermissionRepository = unitOfWork.GetRepository<IUserPermissionRepository>();
        this.authorizationService = authorizationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the list of user permissions stored in the repository
    /// </summary>
    /// <returns>A list of user permissions</returns>
    public async Task<ErrorOr<IEnumerable<UserPermissionDto>>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        // check if the user has the permission to perform the action, or if they get their own permissions
        if (authorizationService.UserPermissions.CanViewPermissions || request.UserId == request.CurrentUserId)
        {   // get the list of user permissions from the repository
            var resultSelectUserRoles = await userPermissionRepository.GetByIdAsync(request.UserId.ToString());
            if (resultSelectUserRoles.Error is null)
            {
                if (resultSelectUserRoles.Data is not null)
                    return resultSelectUserRoles.Data.Adapt<UserPermissionDto[]>();
                else
                    return Array.Empty<UserPermissionDto>();
            }
            else
                return Errors.DataAccess.GetUserRolesError;
        }
        else
            return Errors.Authorization.InvalidPermissionError;
    }
    #endregion
}