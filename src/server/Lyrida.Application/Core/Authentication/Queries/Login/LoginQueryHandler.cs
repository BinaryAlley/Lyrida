#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using Mapster;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.DataAccess.Repositories.Users;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Infrastructure.Core.Authentication;
using Lyrida.Application.Common.DTO.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Queries.Login;

/// <summary>
/// Authentication login query handler
/// </summary>
/// <remarks>
/// Creation Date: 18th of July, 2023
/// </remarks>
public class LoginCommandHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResultDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ISecurity securityService;
    private readonly IUserRepository userRepository;
    private readonly IJwtTokenGenerator jwtTokenGenerator;
    private readonly ITotpTokenGenerator totpTokenGenerator;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="jwtTokenGenerator">Injected service for generating JWT tokens</param>
    /// <param name="totpTokenGenerator">Injected service for generating TOTP tokens</param>
    /// <param name="securityService">Injected service for security related functionality</param>
    public LoginCommandHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator, ITotpTokenGenerator totpTokenGenerator, ISecurity securityService)
    {
        this.securityService = securityService;
        this.jwtTokenGenerator = jwtTokenGenerator;
        this.totpTokenGenerator = totpTokenGenerator;
        userRepository = unitOfWork.GetRepository<IUserRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Verifies the provided credentials against the credentials stored in the repository
    /// </summary>
    /// <param name="query">DTO containing the credentials to be verified</param>
    /// <returns>A DTO containing the login result</returns>
    public async Task<ErrorOr<AuthenticationResultDto>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        // validate that the user exists
        var resultSelectUser = await userRepository.GetByEmailAsync(query.Email);
        if (resultSelectUser.Error is null)
        {
            if (resultSelectUser.Data is not null)
            {
                // validate the password is correct
                if (!securityService.HashService.CheckStringAgainstHash(query.Password, Uri.UnescapeDataString(resultSelectUser.Data[0].Password!)))
                    return Errors.Authentication.InvalidUsername;
                // create the JWT token
                var token = jwtTokenGenerator.GenerateToken(resultSelectUser.Data[0].Id.ToString(), resultSelectUser.Data[0].FirstName, resultSelectUser.Data[0].LastName, resultSelectUser.Data[0].Email);
                // convert the user, and see if they use TOTP
                var convertedUser = resultSelectUser.Data[0].Adapt<UserDto>();
                bool usesTotp = resultSelectUser.Data[0].TotpSecret != null;
                if (usesTotp && query.TotpCode is not null)
                {
                    if (!totpTokenGenerator.ValidateToken(Convert.FromBase64String(securityService.CryptographyService.Decrypt(resultSelectUser.Data[0].TotpSecret!)), query.TotpCode))
                        return Errors.Authentication.InvalidTotpCode;
                }
                convertedUser.UsesTotp = usesTotp;
                convertedUser.Password = string.Empty; // do not include the password in the returned result
                convertedUser.TotpSecret = string.Empty;
                return new AuthenticationResultDto(convertedUser, token);
            }
            else
                return Errors.Authentication.InvalidUsername;
        }
        else
            return Errors.DataAccess.GetUserError;
    }
    #endregion
}