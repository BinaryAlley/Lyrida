#region ========================================================================= USING =====================================================================================
using MediatR;
using Mapster;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.DataAccess.UoW;
using Lyrida.Domain.Common.Errors;
using Lyrida.Application.Core.Authorization;
using Lyrida.Application.Common.DTO.Configuration;
using Lyrida.DataAccess.Repositories.Configuration;
#endregion

namespace Lyrida.Application.Core.Configuration.Commands.Update;

/// <summary>
/// Update profile preference command handler
/// </summary>
/// <remarks>
/// Creation Date: 14th of August, 2023
/// </remarks>
public class UpdateProfilePreferenceCommandHandler : IRequestHandler<UpdateProfilePreferenceCommand, ErrorOr<ProfilePreferencesDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserPreferenceRepository userPreferenceRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    public UpdateProfilePreferenceCommandHandler(IUnitOfWork unitOfWork)
    {
        userPreferenceRepository = unitOfWork.GetRepository<IUserPreferenceRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Updates a profile's preferences in the repository
    /// </summary>
    /// <returns>True, if the action was successful, an Error otherwise</returns>
    public async Task<ErrorOr<ProfilePreferencesDto>> Handle(UpdateProfilePreferenceCommand command, CancellationToken cancellationToken)
    {
        // create the repository DTO and assign its user id
        var userPreference = command.ProfilePreferences.ToStorageDto();
        userPreference.UserId = command.UserId;
        // update the profile preferences
        var resultUpdateUserPreferences = await userPreferenceRepository.UpdateAsync(userPreference);
        // and return them if all went well
        if (resultUpdateUserPreferences.Error is null)
        {
            var resultSelectUserPreferences = await userPreferenceRepository.GetByIdAsync(command.UserId.ToString());
            if (resultSelectUserPreferences.Error is null && resultSelectUserPreferences.Data is not null)
                return resultSelectUserPreferences.Data[0].Adapt<ProfilePreferencesDto>();
            else
                return Errors.DataAccess.GetUserPreferencesError;
        }
        else
            return Errors.DataAccess.InsertUserPreferencesError;
    }
    #endregion
}