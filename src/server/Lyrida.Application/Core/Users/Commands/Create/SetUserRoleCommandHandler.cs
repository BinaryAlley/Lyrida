#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.Application.Core.Authorization;
using Lyrida.DataAccess.Repositories.UserRoles;
using Lyrida.Application.Common.DTO.Authorization;
#endregion

namespace Lyrida.Application.Core.Users.Commands.Create;

/// <summary>
/// Set user role command handler
/// </summary>
/// <remarks>
/// Creation Date: 11th of August, 2023
/// </remarks>
public class SetUserRoleCommandHandler : IRequestHandler<SetUserRoleCommand, ErrorOr<bool>>
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
    public SetUserRoleCommandHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
    {
        userRoleRepository = unitOfWork.GetRepository<IUserRoleRepository>();
        this.authorizationService = authorizationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Sets the role of a user in the repository
    /// </summary>
    /// <returns>True, if the action was successful, an Error otherwise</returns>
    public async Task<ErrorOr<bool>> Handle(SetUserRoleCommand request, CancellationToken cancellationToken)
    {
        // check if the user has the permission to perform the action
        if (authorizationService.UserPermissions.CanViewPermissions)
        {
            // create the updated user role
            var userRole = new UserRoleDto
            {
                UserId = request.UserId,
                RoleId = request.RoleId ?? 0
            };
            var resultUpdateUserRole = await userRoleRepository.UpdateAsync(userRole.ToStorageDto());
            if (resultUpdateUserRole.Error is null)
                return true;
            else
            {
                if (resultUpdateUserRole.Error == "Cannot set admin role!")
                    return Errors.Authorization.SetAdminRoleError;
                else
                    return Errors.DataAccess.UpdateUserRoleError;
            }
        }
        else
            return Errors.Authorization.InvalidPermissionError;
    }
    #endregion
}