#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.Infrastructure.Common.Time;
using Lyrida.DataAccess.Repositories.Users;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Infrastructure.Core.Authentication;
using Lyrida.Application.Common.DTO.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.ChangePassword;

/// <summary>
/// Change password command handler
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ErrorOr<RegistrationResultDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IHash hashService;
    private readonly IUserRepository userRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="hashService">Injected service for credentials hashing</param>
    public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, IHash hashService)
    {
        this.hashService = hashService;
        userRepository = unitOfWork.GetRepository<IUserRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Changes the password of an account in the repository
    /// </summary>
    /// <param name="command">The account whose password is changed</param>
    /// <returns>A DTO containing the password change result</returns>
    public async Task<ErrorOr<RegistrationResultDto>> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        // check if the user already exists
        var resultSelectUser = await userRepository.GetByUsernameAsync(command.Username);
        if (string.IsNullOrEmpty(resultSelectUser.Error))
        {
            if (resultSelectUser.Data is not null) // the account must exist before changing password
            {
                // check if current password is correct!
                if (!hashService.CheckStringAgainstHash(command.CurrentPassword, Uri.UnescapeDataString(resultSelectUser.Data[0].Password!)))
                    return Errors.Authentication.InvalidUsername;
                // hash the new password and assign it
                resultSelectUser.Data[0].Password = Uri.EscapeDataString(hashService.HashString(command.NewPassword));
                var user = new UserDto
                {
                    Username = command.Username
                };
                // update the user
                var resultUpdateUser = await userRepository.UpdateAsync(resultSelectUser.Data[0]);
                if (!string.IsNullOrEmpty(resultUpdateUser.Error) || resultUpdateUser.Count == 0)
                    return Errors.DataAccess.UpdateUserError;
                return new RegistrationResultDto(user);
            }
            else
                return Errors.Authentication.InvalidUsername;
        }
        else
            return Errors.DataAccess.GetUserError;
    }
    #endregion
}