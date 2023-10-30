#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using MediatR;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.DataAccess.Repositories.Users;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Infrastructure.Core.Authentication;
using Lyrida.Application.Common.DTO.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.GenerateTotpQr;

/// <summary>
/// Generate TOTP QR command handler
/// </summary>
/// <remarks>
/// Creation Date: 16th of October, 2023
/// </remarks>
public class GenerateTotpQrCommandHandler : IRequestHandler<GenerateTotpQrCommand, ErrorOr<QrCodeResultDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ISecurity securityService;
    private readonly IUserRepository userRepository;
    private readonly IQRCodeGenerator qrCodeGenerator;
    private readonly ITotpTokenGenerator totpTokenGenerator;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="totpTokenGenerator">Injected service for generating TOTP tokens</param>
    /// <param name="qrCodeGenerator">Injected service for generating QR codes</param>
    /// <param name="securityService">Injected service for security related functionality</param>
    public GenerateTotpQrCommandHandler(IUnitOfWork unitOfWork, ITotpTokenGenerator totpTokenGenerator, IQRCodeGenerator qrCodeGenerator, ISecurity securityService)
    {
        this.securityService = securityService;
        this.qrCodeGenerator = qrCodeGenerator;
        this.totpTokenGenerator = totpTokenGenerator;
        userRepository = unitOfWork.GetRepository<IUserRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Handles the execution of the <see cref="GenerateTotpQrCommand"/>, which is responsible for generating a TOTP QR code.
    /// </summary>
    /// <param name="command">The <see cref="GenerateTotpQrCommand"/> containing the email for which a QR code needs to be generated.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/> to observe while waiting for tasks to complete.</param>
    /// <returns>
    /// A Task containing an error or a QR code result DTO
    /// </returns>
    public async Task<ErrorOr<QrCodeResultDto>> Handle(GenerateTotpQrCommand command, CancellationToken cancellationToken)
    {
        // first, ensure the user exists in the system
        var userResult = await userRepository.GetByIdAsync(command.UserId.ToString());
        if (userResult.Error is not null || userResult.Data is null)
            return Errors.DataAccess.GetUserError;
        // generate a TOTP secret
        byte[] secret = totpTokenGenerator.GenerateSecret();
        // convert the secret into a QR code
        string dataUri = qrCodeGenerator.GenerateQrCodeDataUri(userResult.Data[0].Email, secret);

        userResult.Data[0].TotpSecret = securityService.CryptographyService.Encrypt(Convert.ToBase64String(secret));
        // update the user's totp secret
        var resultInsertUser = await userRepository.UpdateAsync(userResult.Data[0]);
        if (!string.IsNullOrEmpty(resultInsertUser.Error))
            return Errors.DataAccess.UpdateUserError;

        return new QrCodeResultDto(dataUri);
    }
    #endregion
}