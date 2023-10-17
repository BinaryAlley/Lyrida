#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using Lyrida.Application.Common.Entities.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.GenerateTotpQr;

/// <summary>
/// Generate TOTP QR command
/// </summary>
/// <remarks>
/// Creation Date: 16th of October, 2023
/// </remarks>
public record GenerateTotpQrCommand(string Email) : IRequest<ErrorOr<QrCodeResultEntity>>;