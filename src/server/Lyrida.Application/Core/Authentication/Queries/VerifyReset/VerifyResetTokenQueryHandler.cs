#region ========================================================================= USING =====================================================================================
using MediatR;
using Mapster;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.DataAccess.UoW;
using Lyrida.Infrastructure.Common.Time;
using Lyrida.DataAccess.Repositories.Users;
using Lyrida.Application.Common.Errors.Types;
using Lyrida.Infrastructure.Core.Authentication;
using Lyrida.Application.Common.Entities.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Queries.VerifyReset;

/// <summary>
/// Password reset token verification query handler
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public class VerifyResetTokenQueryHandler : IRequestHandler<VerifyResetTokenQuery, ErrorOr<AuthenticationResultEntity>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ITimeService timeService;
    private readonly IUserRepository userRepository;
    private readonly IJwtTokenGenerator jwtTokenGenerator;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="jwtTokenGenerator">Injected service for generating JWT tokens</param>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="timeService">Injected service for time related functionality</param>
    public VerifyResetTokenQueryHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator, ITimeService timeService)
    {
        this.jwtTokenGenerator = jwtTokenGenerator;
        this.timeService = timeService;
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
    public async Task<ErrorOr<AuthenticationResultEntity>> Handle(VerifyResetTokenQuery query, CancellationToken cancellationToken)
    {
        // validate that an account for the provided token exists
        var resultSelectUser = await userRepository.GetByValidationTokenAsync(query.Token);
        if (resultSelectUser.Error is null && resultSelectUser.Data is not null)
        {
            if (resultSelectUser.Data[0].IsVerified) // only verified accounts should be able to reset their passwords
            {
                // make sure the token was generated in the last 24 hours
                if (timeService.Now.Subtract(resultSelectUser.Data[0].VerificationTokenCreated!.Value).TotalHours < 24) 
                {
                    // disable the token for further validations
                    resultSelectUser.Data[0].VerificationToken = null;
                    resultSelectUser.Data[0].VerificationTokenCreated = null;
                    var verify = await userRepository.UpdateAsync(resultSelectUser.Data[0]);
                    if (!string.IsNullOrEmpty(verify.Error))
                        return Errors.DataAccess.ValidatePasswordResetError;
                }
                else
                    return Errors.Authentication.TokenExpired;
            }
            else
                return Errors.Authentication.UnverifiedAccount;
            resultSelectUser.Data[0].Password = string.Empty; // do not include the password in the returned result
            // create the JWT token
            var token = jwtTokenGenerator.GenerateToken(resultSelectUser.Data[0].Id.ToString(), resultSelectUser.Data[0].FirstName, resultSelectUser.Data[0].LastName, resultSelectUser.Data[0].Email);
            return new AuthenticationResultEntity(resultSelectUser.Data[0].Adapt<UserEntity>(), token);
        }
        else
            return Errors.Authentication.TokenExpired;
    }
    #endregion
}