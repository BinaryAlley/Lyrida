#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.Entities.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Queries.VerifyReset;

/// <summary>
/// Password reset token verification query
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public record VerifyResetTokenQuery(string Token) : IRequest<ErrorOr<AuthenticationResultEntity>>;