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
using Lyrida.DataAccess.Repositories.UserRoles;
using Lyrida.Application.Common.DTO.Authorization;
#endregion

namespace Lyrida.Application.Core.Users.Queries.Read;

/// <summary>
/// Get user roles query handler
/// </summary>
/// <remarks>
/// Creation Date: 18th of August, 2023
/// </remarks>
public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, ErrorOr<IEnumerable<UserRoleDto>>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserRoleRepository userRoleRepository;
    private readonly IAuthorizationService authorizationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="authorizationService">Injected service for permissions</param>
    public GetUserRolesQueryHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
    {
        userRoleRepository = unitOfWork.GetRepository<IUserRoleRepository>();
        this.authorizationService = authorizationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the list of user roles stored in the repository
    /// </summary>
    /// <returns>A list of user roles</returns>
    public async Task<ErrorOr<IEnumerable<UserRoleDto>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        // check if the user has the permission to perform the action or if they get their own roles
        if (authorizationService.UserPermissions.CanViewPermissions || request.UserId == request.CurrentUserId)
        { // get the list of user roles from the repository
            var resultSelectUserRoles = await userRoleRepository.GetByUserIdAsync(request.UserId.ToString());
            if (resultSelectUserRoles.Error is null)
            {
                if (resultSelectUserRoles.Data is not null)
                    return resultSelectUserRoles.Data.Adapt<UserRoleDto[]>();
                else
                    return Array.Empty<UserRoleDto>();
            }
            else
                return Errors.DataAccess.GetUserRolesError;
        }
        else
            return Errors.Authorization.InvalidPermissionError;
    }
    #endregion
}