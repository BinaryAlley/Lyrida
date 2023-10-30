#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.DTO.Configuration;
#endregion

namespace Lyrida.Application.Core.Configuration.Commands.Update;

/// <summary>
/// Set user profile preferences command
/// </summary>
/// <remarks>
/// Creation Date: 25th of October, 2023
/// </remarks>
public record UpdateProfilePreferenceCommand(int UserId, ProfilePreferencesDto ProfilePreferences) : IRequest<ErrorOr<ProfilePreferencesDto>>;