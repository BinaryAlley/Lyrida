#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Linq;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.DataAccess.Repositories.Users;
using Lyrida.DataAccess.Repositories.Roles;
using Lyrida.Application.Core.Authorization;
using Lyrida.DataAccess.Repositories.UserRoles;
#endregion

namespace Lyrida.Application.Core.Users.Commands.Delete;

/// <summary>
/// Delete user command handler
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ErrorOr<bool>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserRepository userRepository;
    private readonly IRoleRepository roleRepository;
    private readonly IUserRoleRepository userRoleRepository;
    private readonly IAuthorizationService authorizationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="authorizationService">Injected service for permissions</param>
    public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
    {
        userRepository = unitOfWork.GetRepository<IUserRepository>();
        roleRepository = unitOfWork.GetRepository<IRoleRepository>();
        userRoleRepository = unitOfWork.GetRepository<IUserRoleRepository>();
        this.authorizationService = authorizationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Deletes a user in the repository
    /// </summary>
    /// <returns>True, if the deletion was successful, an Error otherwise</returns>
    public async Task<ErrorOr<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        // check if the user has the permission to perform the action
        if (authorizationService.UserPermissions.CanEditUsers)
        {
            // get the id of the Admin role
            var resultSelectAdminRole = await roleRepository.GetByNameAsync("Admin");
            if (resultSelectAdminRole.Error is null && resultSelectAdminRole.Data is not null)
            {
                // get the roles of the user to be deleted
                var resultSelectUserRole = await userRoleRepository.GetByUserIdAsync(request.Id.ToString());
                if (resultSelectUserRole.Error is null)
                {
                    // if the user to be deleted is in the Admin role, return error - admin account can't be deleted!
                    if (resultSelectUserRole.Data?.Any(userRole => userRole.Id == resultSelectAdminRole.Data[0].Id && userRole.UserId == request.Id) == true)
                        return Errors.Authorization.DeleteAdminAccountError;
                }
                else
                    return Errors.DataAccess.GetUserRolesError;
            }
            else
                return Errors.DataAccess.GetRoleError;
            // delete the user
            var resultDeleteUser = await userRepository.DeleteByIdAsync(request.Id.ToString());
            if (resultDeleteUser.Error is null && resultDeleteUser.Count > 0)
                return true;
            else
                return Errors.DataAccess.DeleteUserError;
        }
        else
            return Errors.Authorization.InvalidPermissionError;
    }
    #endregion
}