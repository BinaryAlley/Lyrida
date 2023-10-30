#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using Mapster;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.DataAccess.UoW;
using System.Collections.Generic;
using Lyrida.Domain.Common.Errors;
using Lyrida.DataAccess.Repositories.Roles;
using Lyrida.Application.Core.Authorization;
using Lyrida.Application.Common.DTO.Authorization;
#endregion

namespace Lyrida.Application.Core.Roles.Queries.Read;

/// <summary>
/// Get roles query handler
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, ErrorOr<IEnumerable<RoleDto>>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IRoleRepository roleRepository;
    private readonly IAuthorizationService authorizationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="authorizationService">Injected service for permissions</param>
    public GetAllRolesQueryHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
    {
        roleRepository = unitOfWork.GetRepository<IRoleRepository>();
        this.authorizationService = authorizationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the list of roles stored in the repository
    /// </summary>
    /// <returns>A list of roles</returns>
    public async Task<ErrorOr<IEnumerable<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        // check if the user has the permission to perform the action
        if (authorizationService.UserPermissions.CanViewPermissions)
        {
            // get the list of roles from the repository
            var resultSelectRoles = await roleRepository.GetAllAsync();
            if (resultSelectRoles.Error is null)
            {
                if (resultSelectRoles.Data is not null)
                    return resultSelectRoles.Data.Adapt<RoleDto[]>();
                else
                    return Array.Empty<RoleDto>();
            }
            else
                return Errors.DataAccess.GetRolesError;
        }
        else
            return Errors.Authorization.InvalidPermissionError;
    }
    #endregion
}