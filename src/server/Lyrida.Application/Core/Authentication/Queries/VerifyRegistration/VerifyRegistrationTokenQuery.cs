#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.Entities.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Queries.VerifyRegistration;

/// <summary>
/// Registration token verification query
/// </summary>
/// <remarks>
/// Creation Date: 27th of July, 2023
/// </remarks>
public record VerifyRegistrationTokenQuery(string Token) : IRequest<ErrorOr<AuthenticationResultEntity>>;