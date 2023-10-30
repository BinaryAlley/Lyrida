#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.DTO.Configuration;
#endregion

namespace Lyrida.Application.Core.Configuration.Queries.Read;

/// <summary>
/// Query for getting the profile preferences of an account
/// </summary>
/// <remarks>
/// Creation Date: 25th of October, 2023
/// </remarks>
public record GetProfilePreferencesQuery(int UserId) : IRequest<ErrorOr<ProfilePreferencesDto>>;
