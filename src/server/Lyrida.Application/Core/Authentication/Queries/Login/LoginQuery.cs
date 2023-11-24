#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.DTO.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Queries.Login;

/// <summary>
/// Authentication login query
/// </summary>
/// <remarks>
/// Creation Date: 18th of July, 2023
/// </remarks>
public record LoginQuery(string Username, string Password, string TotpCode) : IRequest<ErrorOr<AuthenticationResultDto>>;