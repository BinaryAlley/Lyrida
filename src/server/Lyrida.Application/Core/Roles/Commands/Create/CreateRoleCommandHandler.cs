#region ========================================================================= USING =====================================================================================
using MediatR;
using Mapster;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.DataAccess.UoW;
using Lyrida.DataAccess.Repositories.Roles;
using Lyrida.Application.Core.Authorization;
using Lyrida.Application.Common.Errors.Types;
using Lyrida.Domain.Common.Entities.Authorization;
#endregion

namespace Lyrida.Application.Core.Roles.Commands.Create;

/// <summary>
/// Create role command handler
/// </summary>
/// <remarks>
/// Creation Date: 14th of August, 2023
/// </remarks>
public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ErrorOr<RoleEntity>>
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
    public CreateRoleCommandHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
    {
        roleRepository = unitOfWork.GetRepository<IRoleRepository>();
        this.authorizationService = authorizationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates a role in the repository
    /// </summary>
    /// <returns>True, if the action was successful, an Error otherwise</returns>
    public async Task<ErrorOr<RoleEntity>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        // check if the user has the permission to perform the action
        if (authorizationService.UserPermissions.CanViewPermissions)
        {
            // make sure the role does not already exist
            var resultCheckRoleExists = await roleRepository.GetByNameAsync(request.RoleName!);
            if (resultCheckRoleExists.Error is null)
            {
                if (resultCheckRoleExists.Count == 0) // if there are no roles with this name
                {
                    // insert a new role
                    var resultInsertRole = await roleRepository.InsertAsync(request.RoleName!, request.Permissions);
                    // and return it if all went well
                    if (resultInsertRole.Error is null && resultInsertRole.Data is not null)
                        return resultInsertRole.Data[0].Adapt<RoleEntity>();
                    else
                        return Errors.DataAccess.InsertRoleError;
                }
                else
                    return Errors.Authorization.RoleAlreadyExists;
            }
            else
                return Errors.DataAccess.GetRoleError;
        }
        else
            return Errors.Authorization.InvalidPermission;
    }
    #endregion
}