#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.DataAccess.UoW;
using Lyrida.DataAccess.Repositories.Roles;
using Lyrida.Application.Core.Authorization;
using Lyrida.Application.Common.Errors.Types;
#endregion

namespace Lyrida.Application.Core.Roles.Commands.Delete;

/// <summary>
/// Delete role command handler
/// </summary>
/// <remarks>
/// Creation Date: 17th of August, 2023
/// </remarks>
public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, ErrorOr<bool>>
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
    public DeleteRoleCommandHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
    {
        roleRepository = unitOfWork.GetRepository<IRoleRepository>();
        this.authorizationService = authorizationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Deletes a role in the repository
    /// </summary>
    /// <returns>True, if the deletion was successful, an Error otherwise</returns>
    public async Task<ErrorOr<bool>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        // check if the user has the permission to perform the action
        if (authorizationService.UserPermissions.CanViewPermissions)
        {
            // get the id of the Admin role
            var resultSelectAdminRole = await roleRepository.GetByNameAsync("Admin");
            if (resultSelectAdminRole.Error is null && resultSelectAdminRole.Data is not null)
            {
                // if the role to be deleted is in the Admin role, return error - admin role can't be deleted!
                if (resultSelectAdminRole.Data[0].Id == request.Id)
                    return Errors.Authorization.CannotDeleteAdminRole;
            }
            else
                return Errors.DataAccess.GetRoleError;
            // delete the role
            var resultDeleteRole = await roleRepository.DeleteByIdAsync(request.Id.ToString());
            if (resultDeleteRole.Error is null && resultDeleteRole.Count > 0)
                return true;
            else
                return Errors.DataAccess.DeleteRoleError;
        }
        else
            return Errors.Authorization.InvalidPermission;
    }
    #endregion
}