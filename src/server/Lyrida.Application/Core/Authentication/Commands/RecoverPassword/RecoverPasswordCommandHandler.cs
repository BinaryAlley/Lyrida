#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.DataAccess.UoW;
using Lyrida.Infrastructure.Localization;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.DataAccess.Repositories.Users;
using Lyrida.Application.Common.Errors.Types;
using Lyrida.Infrastructure.Core.Authentication;
using Lyrida.Infrastructure.Common.Notification;
using Lyrida.Application.Common.Entities.Common;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.RecoverPassword;

/// <summary>
/// Recover password command handler
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public class RecoverPasswordCommandHandler : IRequestHandler<RecoverPasswordCommand, ErrorOr<CommandResultEntity>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IEmailService emailService;
    private readonly IUserRepository userRepository;
    private readonly ITokenGenerator tokenGenerator;
    private readonly ITranslationService translationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="tokenGenerator">Injected service for generating authentication tokens</param>
    /// <param name="emailService">Injected service for sending emails</param>
    public RecoverPasswordCommandHandler(IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator, IEmailService emailService, ITranslationService translationService)
    {
        this.tokenGenerator = tokenGenerator;
        this.emailService = emailService;
        this.translationService = translationService;
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
    public async Task<ErrorOr<CommandResultEntity>> Handle(RecoverPasswordCommand command, CancellationToken cancellationToken)
    {
        // check if the provided email is registered as an account
        var resultSelectUser = await userRepository.GetByEmailAsync(command.Email);
        if (resultSelectUser.Error is null && resultSelectUser.Data is not null)
        {
            // check if a token was not already issued and not verified by the user
            if (string.IsNullOrEmpty(resultSelectUser.Data[0].VerificationToken))
            {
                string language = translationService.Language == Language.English ? "en" : translationService.Language == Language.German ? "de" : "ro";
                // generate a new validation token and assign it to the user - they need to be able to prove they have access to the e-mail account
                resultSelectUser.Data[0].VerificationToken = tokenGenerator.GenerateToken();
                resultSelectUser.Data[0].VerificationTokenCreated = DateTime.Now;
                // update the user in the repository with the generated token
                var resultUpdateUser = await userRepository.UpdateAsync(resultSelectUser.Data[0]);
                if (!string.IsNullOrEmpty(resultUpdateUser.Error) || resultUpdateUser.Count == 0)
                    return Errors.DataAccess.UpdateUserError;
                else // send an email to the user and ask them to confirm by clicking the validation link
                    await emailService.SendEmailAsync(translationService.Translate(Terms.PasswordReset), translationService.Translate(Terms.PasswordChangeEmail1)
                            + "<a href=\"https://www.thefibremanager.com/Account/ResetPassword?token=" + resultSelectUser.Data[0].VerificationToken + "&lang=" + language + "\" target=\"_blank\">" +
                            translationService.Translate(Terms.ClickHereToChangePassword) + "</a>" + translationService.Translate(Terms.PasswordChangeEmail2), "email address here", command.Email);
                return new CommandResultEntity(true);
            }
            else
                return Errors.Authentication.TokenAlreadyIssued;
        }
        else
            return Errors.DataAccess.GetUserError;
    }
    #endregion
}