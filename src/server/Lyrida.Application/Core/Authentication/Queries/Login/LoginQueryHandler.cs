#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using Mapster;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.DataAccess.Repositories.Users;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Application.Common.Errors.Types;
using Lyrida.Infrastructure.Core.Authentication;
using Lyrida.Application.Common.Entities.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Queries.Login;

/// <summary>
/// Authentication login query handler
/// </summary>
/// <remarks>
/// Creation Date: 18th of July, 2023
/// </remarks>
public class LoginCommandHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResultEntity>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IHash hashService;
    private readonly IUserRepository userRepository;
    private readonly IJwtTokenGenerator jwtTokenGenerator;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="jwtTokenGenerator">Injected service for generating JWT tokens</param>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="hashService">Injected service for credentials hashing</param>
    public LoginCommandHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator, IHash hashService)
    {
        this.hashService = hashService;
        this.jwtTokenGenerator = jwtTokenGenerator;
        userRepository = unitOfWork.GetRepository<IUserRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Verifies the provided credentials against the credentials stored in the repository
    /// </summary>
    /// <param name="email">The email to verify</param>
    /// <param name="password">The password to verify</param>
    /// <returns>An entity containing the login result</returns>
    public async Task<ErrorOr<AuthenticationResultEntity>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        // validate that the user exists
        var resultSelectUser = await userRepository.GetByEmailAsync(query.Email);
        if (resultSelectUser.Error is null)
        {
            if (resultSelectUser.Data is not null)
            {
                // validate the password is correct
                if (!hashService.CheckStringAgainstHash(query.Password, Uri.UnescapeDataString(resultSelectUser.Data[0].Password!)))
                    return Errors.Authentication.InvalidUsername;
                // validate the account si activated
                if (!resultSelectUser.Data[0].IsVerified)
                    return Errors.Authentication.UnverifiedAccount;
                resultSelectUser.Data[0].Password = string.Empty; // do not include the password in the returned result
                // create the JWT token
                var token = jwtTokenGenerator.GenerateToken(resultSelectUser.Data[0].Id.ToString(), resultSelectUser.Data[0].FirstName, resultSelectUser.Data[0].LastName, resultSelectUser.Data[0].Email);
                return new AuthenticationResultEntity(resultSelectUser.Data[0].Adapt<UserEntity>(), token);
            }
            else
                return Errors.Authentication.InvalidUsername;
        }
        else
            return Errors.DataAccess.GetUserError;
    }
    #endregion
}