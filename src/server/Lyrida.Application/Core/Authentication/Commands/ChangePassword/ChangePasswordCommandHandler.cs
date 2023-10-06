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

namespace Lyrida.Application.Core.Authentication.Commands.ChangePassword;

/// <summary>
/// Change password command handler
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ErrorOr<RegistrationResultEntity>>
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
    public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator, IHash hashService, ITimeService timeService)
    {
        this.hashService = hashService;
        this.timeService = timeService;
        this.tokenGenerator = tokenGenerator;
        userRepository = unitOfWork.GetRepository<IUserRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Registers a new account in the repository
    /// </summary>
    /// <param name="command">The account to be registered</param>
    /// <param name="lastName">The last name of the account to be registered</param>
    /// <param name="email">The email of the account to be registered</param>
    /// <param name="password">The password of the account to be registered</param>
    /// <returns>An entity containing the register result</returns>
    public async Task<ErrorOr<RegistrationResultEntity>> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        // check if the user already exists
        var resultSelectUser = await userRepository.GetByEmailAsync(command.Email);
        if (string.IsNullOrEmpty(resultSelectUser.Error))
        {
            if (resultSelectUser.Data is not null) // the account must exist before changing password
            {
                // check if current password is correct!
                if (!hashService.CheckStringAgainstHash(command.CurrentPassword, Uri.UnescapeDataString(resultSelectUser.Data[0].Password!)))
                    return Errors.Authentication.InvalidUsername;
                // hash the new password and assign it
                resultSelectUser.Data[0].Password = Uri.EscapeDataString(hashService.HashString(command.NewPassword));
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