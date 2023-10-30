#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.DataAccess.UoW;
using Lyrida.Domain.Common.Errors;
using Lyrida.DataAccess.Repositories.Users;
using Lyrida.Application.Common.DTO.Common;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Infrastructure.Core.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.RecoverPassword;

/// <summary>
/// Recover password command handler
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public class RecoverPasswordCommandHandler : IRequestHandler<RecoverPasswordCommand, ErrorOr<CommandResultDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ISecurity securityService;
    private readonly IUserRepository userRepository;
    private readonly ITotpTokenGenerator totpTokenGenerator;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="totpTokenGenerator">Injected service for generating TOTP tokens</param>
    /// <param name="securityService">Injected service for security related functionality</param>
    public RecoverPasswordCommandHandler(IUnitOfWork unitOfWork, ITotpTokenGenerator totpTokenGenerator, ISecurity securityService)
    {
        this.securityService = securityService;
        this.totpTokenGenerator = totpTokenGenerator;
        userRepository = unitOfWork.GetRepository<IUserRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Handles the execution of the RecoverPasswordCommand, which is responsible for recovering the user's password.
    /// </summary>
    /// <param name="command">The RecoverPasswordCommand containing the necessary data to recover the password.</param>
    /// <param name="cancellationToken">An optional CancellationToken to observe while waiting for tasks to complete.</param>
    /// <returns>
    /// A Task containing an error or a command result
    /// </returns>
    public async Task<ErrorOr<CommandResultDto>> Handle(RecoverPasswordCommand command, CancellationToken cancellationToken)
    {
        // validate that the user exists
        var resultSelectUser = await userRepository.GetByEmailAsync(command.Email);
        if (resultSelectUser.Error is null)
        {
            if (resultSelectUser.Data is not null)
            {
                // convert the user, and see if they use TOTP
                if (!totpTokenGenerator.ValidateToken(Convert.FromBase64String(securityService.CryptographyService.Decrypt(resultSelectUser.Data[0].TotpSecret!)), command.TotpCode))
                    return Errors.Authentication.InvalidTotpCode;
                // hash the new password and assign it
                resultSelectUser.Data[0].Password = Uri.EscapeDataString(securityService.HashService.HashString("Abcd123$"));
                // update the user
                var resultUpdateUser = await userRepository.UpdateAsync(resultSelectUser.Data[0]);
                if (!string.IsNullOrEmpty(resultUpdateUser.Error) || resultUpdateUser.Count == 0)
                    return Errors.DataAccess.UpdateUserError;
                return new CommandResultDto(true);
            }
            else
                return Errors.Authentication.InvalidUsername;
        }
        else
            return Errors.DataAccess.GetUserError;
    }
    #endregion
}