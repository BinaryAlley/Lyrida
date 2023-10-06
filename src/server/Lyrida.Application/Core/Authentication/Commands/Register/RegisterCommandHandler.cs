#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.DataAccess.UoW;
using Lyrida.Infrastructure.Common.Time;
using Lyrida.DataAccess.Repositories.Users;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Application.Common.Errors.Types;
using Lyrida.Infrastructure.Core.Authentication;
using Lyrida.Application.Common.Entities.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.Register;

/// <summary>
/// Authentication register command handler
/// </summary>
/// <remarks>
/// Creation Date: 18th of July, 2023
/// </remarks>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<RegistrationResultEntity>>
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
    public RegisterCommandHandler(IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator, IHash hashService, ITimeService timeService)
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
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>An entity containing the register result, or an error</returns>
    public async Task<ErrorOr<RegistrationResultEntity>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // check if the user already exists
        var resultSelectUser = await userRepository.GetByEmailAsync(command.Email);
        if (resultSelectUser.Error is null)
        {
            if (resultSelectUser.Data is not null)
                return Errors.Authentication.DuplicateEmailError;
        }
        else
            return Errors.DataAccess.GetUserError;

        var user = new UserEntity
        {
            Email = command.Email,
            Password = Uri.EscapeDataString(hashService.HashString(command.Password)),
            LastName = command.LastName,
            FirstName = command.FirstName,
            VerificationTokenCreated = timeService.Now, 
            VerificationToken = tokenGenerator.GenerateToken()
        };
        // insert the user
        var resultInsertUser = await userRepository.InsertAsync(user.ToStorageEntity());
        if (!string.IsNullOrEmpty(resultInsertUser.Error) || resultInsertUser.Data is null)
            return Errors.DataAccess.InsertUserError;
        user.Password = string.Empty; // do not include the password in the returned result!
        return new RegistrationResultEntity(user);
    }
    #endregion
}