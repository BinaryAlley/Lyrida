#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.Application.Common.Errors.Types;
using Lyrida.Application.Common.Entities.Authentication;
using Lyrida.DataAccess.Repositories.Users;
using Lyrida.Infrastructure.Core.Authentication;
using Lyrida.DataAccess.UoW;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.GenerateTotpQr;

/// <summary>
/// Generate TOTP QR command handler
/// </summary>
/// <remarks>
/// Creation Date: 16th of October, 2023
/// </remarks>
public class GenerateTotpQrCommandHandler : IRequestHandler<GenerateTotpQrCommand, ErrorOr<QrCodeResultEntity>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
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
    public GenerateTotpQrCommandHandler(IUnitOfWork unitOfWork, ITotpTokenGenerator totpTokenGenerator, IQRCodeGenerator qrCodeGenerator)
    {
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
    /// A Task containing an error or a QR code result entity
    /// </returns>
    public async Task<ErrorOr<QrCodeResultEntity>> Handle(GenerateTotpQrCommand command, CancellationToken cancellationToken)
    {
        // first, ensure the user exists in the system
        var userResult = await userRepository.GetByEmailAsync(command.Email);
        if (userResult.Error is not null || userResult.Data is null)
            return Errors.DataAccess.GetUserError;
        // generate a TOTP secret
        byte[] secret = totpTokenGenerator.GenerateSecret();
        // convert the secret into a QR code
        string dataUri = qrCodeGenerator.GenerateQrCodeDataUri(command.Email, secret);
        return new QrCodeResultEntity(dataUri);
    }
    #endregion
}