#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using Mapster;
using ErrorOr;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.DataAccess.UoW;
using System.Collections.Generic;
using Lyrida.DataAccess.Repositories.Users;
using Lyrida.Application.Core.Authorization;
using Lyrida.Application.Common.Errors.Types;
using Lyrida.Application.Common.Entities.Authentication;
#endregion

namespace Lyrida.Application.Core.Users.Queries.Read;

/// <summary>
/// Get users query handler
/// </summary>
/// <remarks>
/// Creation Date: 04th of August, 2023
/// </remarks>
public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ErrorOr<IEnumerable<UserEntity>>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserRepository userRepository;
    private readonly IAuthorizationService authorizationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="authorizationService">Injected service for permissions</param>
    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
    {
        userRepository = unitOfWork.GetRepository<IUserRepository>();
        this.authorizationService = authorizationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the list of users stored in the repository
    /// </summary>
    /// <returns>A list of users</returns>
    public async Task<ErrorOr<IEnumerable<UserEntity>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        // check if the user has the permission to perform the action
        if (authorizationService.UserPermissions.CanViewUsers)
        { // get the list of users from the repository
            var resultSelectUser = await userRepository.GetAllAsync();
            if (resultSelectUser.Error is null)
            {
                if (resultSelectUser.Data is not null)
                    return resultSelectUser.Data.Select(user =>
                    {
                        user.Password = string.Empty;
                        return user;
                    }).Adapt<UserEntity[]>();
                else
                    return Array.Empty<UserEntity>();
            }
            else
                return Errors.DataAccess.GetUsersError;
        }
        else
            return Errors.Authorization.InvalidPermission;
    }
    #endregion
}