#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using Lyrida.Application.Common.DTO.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.GenerateTotpQr;

/// <summary>
/// Generate TOTP QR command
/// </summary>
/// <remarks>
/// Creation Date: 16th of October, 2023
/// </remarks>
public record GenerateTotpQrCommand(int UserId) : IRequest<ErrorOr<QrCodeResultDto>>;