#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.DataAccess.UoW;
using Lyrida.Application.Core.Authorization;
using Lyrida.Application.Common.Errors.Types;
using Lyrida.DataAccess.Repositories.UserPermissions;
#endregion

namespace Lyrida.Application.Core.Users.Commands.Update;

/// <summary>
/// Set user permissions command handler
/// </summary>
/// <remarks>
/// Creation Date: 18th of August, 2023
/// </remarks>
public class SetUserPermissionsCommandHandler : IRequestHandler<SetUserPermissionsCommand, ErrorOr<bool>>
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
    public SetUserPermissionsCommandHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
    {
        userPermissionRepository = unitOfWork.GetRepository<IUserPermissionRepository>();
        this.authorizationService = authorizationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Sets the permissions of a user in the repository
    /// </summary>
    /// <returns>True, if the action was successful, an Error otherwise</returns>
    public async Task<ErrorOr<bool>> Handle(SetUserPermissionsCommand request, CancellationToken cancellationToken)
    {
        // check if the user has the permission to perform the action
        if (authorizationService.UserPermissions.CanViewPermissions)
        {
            var resultUpdateUserRole = await userPermissionRepository.InsertAsync(request.UserId.ToString(), request.Permissions);
            if (resultUpdateUserRole.Error is null)
                return true;
            else
            {
                if (resultUpdateUserRole.Error == "Cannot update admin permissions!")
                    return Errors.Authorization.CannotUpdateAdminRole;
                else
                    return Errors.DataAccess.UpdateUserPermissionsError;
            }
        }
        else
            return Errors.Authorization.InvalidPermission;
    }
    #endregion
}