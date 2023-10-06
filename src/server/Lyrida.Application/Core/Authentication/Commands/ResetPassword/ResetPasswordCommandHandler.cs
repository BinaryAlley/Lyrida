#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Infrastructure.Common.Time;
using Lyrida.DataAccess.Repositories.Users;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Application.Common.Errors.Types;
using Lyrida.Infrastructure.Core.Authentication;
using Lyrida.Application.Common.Entities.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.ResetPassword;

/// <summary>
/// Authentication password reset command handler
/// </summary>
/// <remarks>
/// Creation Date: 02nd of August, 2023
/// </remarks>
public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ErrorOr<RegistrationResultEntity>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IHash hashService;
    private readonly ITimeService timeService;
    private readonly IUserRepository userRepository;
    private readonly ITokenGenerator tokenGenerator;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="tokenGenerator">Injected service for generating authentication tokens</param>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="hashService">Injected service for credentials hashing</param>
    /// <param name="timeService">Injected service for time related functionality</param>
    public ResetPasswordCommandHandler(IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator, IHash hashService, ITimeService timeService)
    {
        this.hashService = hashService;
        this.timeService = timeService;
        this.tokenGenerator = tokenGenerator;
        userRepository = unitOfWork.GetRepository<IUserRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Resets the password of an account
    /// </summary>
    /// <param name="command">The account whose password is reset</param>
    public async Task<ErrorOr<RegistrationResultEntity>> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        // check if the user already exists
        var resultSelectUser = await userRepository.GetByEmailAsync(command.Email);
        if (string.IsNullOrEmpty(resultSelectUser.Error))
        {
            if (resultSelectUser.Data is not null) // the account must exist before changing password
            {
                resultSelectUser.Data[0].Password = Uri.EscapeDataString(hashService.HashString(command.Password)); 
                var user = new UserEntity
                {
                    Email = command.Email,
                    LastName = resultSelectUser.Data[0].LastName,
                    FirstName = resultSelectUser.Data[0].FirstName,
                    VerificationTokenCreated = timeService.Now, 
                    VerificationToken = tokenGenerator.GenerateToken()
                };
                // update the user
                var resultUpdateUser = await userRepository.UpdateAsync(resultSelectUser.Data[0]);
                if (!string.IsNullOrEmpty(resultUpdateUser.Error) || resultUpdateUser.Count == 0)
                    return Errors.DataAccess.UpdateUserError;
                return new RegistrationResultEntity(user);
            }
            else
                return Errors.Authentication.InvalidUsername;
        }
        else
            return Errors.DataAccess.GetUserError;
    }
    #endregion
}