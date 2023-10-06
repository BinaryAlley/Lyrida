#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.DataAccess.Repositories.Roles;
using Lyrida.Application.Core.Authorization;
using Lyrida.Application.Common.Errors.Types;
#endregion

namespace Lyrida.Application.Core.Roles.Commands.Update;

/// <summary>
/// Update role command handler
/// </summary>
/// <remarks>
/// Creation Date: 18th of August, 2023
/// </remarks>
public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ErrorOr<bool>>
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
    public UpdateRoleCommandHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
    {
        roleRepository = unitOfWork.GetRepository<IRoleRepository>();
        this.authorizationService = authorizationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Updates a role in the respository
    /// </summary>
    /// <returns>True, if the action was successful, an Error otherwise</returns>
    public async Task<ErrorOr<bool>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        // check if the user has the permission to perform the action
        if (authorizationService.UserPermissions.CanViewPermissions)
        {
            // make sure the role does exist
            var resultSelectRole = await roleRepository.GetByIdAsync(request.RoleId.ToString());
            if (resultSelectRole.Error is null)
            {
                if (resultSelectRole.Data is not null)
                {
                    // get the id of the Admin role
                    var resultSelectAdminRole = await roleRepository.GetByNameAsync("Admin");
                    if (resultSelectAdminRole.Error is null && resultSelectAdminRole.Data is not null)
                    {
                        // if the role to be updated is in the Admin role, return error - admin role can't be edited!
                        if (resultSelectAdminRole.Data[0].Id == request.RoleId)
                            return Errors.Authorization.CannotUpdateAdminRole;
                    }
                    else
                        return Errors.DataAccess.GetRoleError;
                    // update the role
                    var resultUpdateRole = await roleRepository.UpdateAsync(request.RoleId.ToString(), request.RoleName!, request.Permissions);
                    if (resultUpdateRole.Error is null && resultUpdateRole.Count > 0)
                        return true;
                    else
                        return Errors.DataAccess.InsertRoleError;
                }
                return Errors.Authorization.RoleDoesNotExist;
            }
            else
                return Errors.DataAccess.GetRoleError;
        }
        else
            return Errors.Authorization.InvalidPermission;
    }
    #endregion
}