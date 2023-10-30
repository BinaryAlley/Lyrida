#region ========================================================================= USING =====================================================================================
using Mapster;
using MediatR;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.Application.Common.DTO.Configuration;
using Lyrida.DataAccess.Repositories.Configuration;
#endregion

namespace Lyrida.Application.Core.Configuration.Queries.Read;

/// <summary>
/// Get profile preferences query handler
/// </summary>
/// <remarks>
/// Creation Date: 25th of August, 2023
/// </remarks>
public class GetProfilePreferencesQueryHandler : IRequestHandler<GetProfilePreferencesQuery, ErrorOr<ProfilePreferencesDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserPreferenceRepository userPreferenceRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    public GetProfilePreferencesQueryHandler(IUnitOfWork unitOfWork)
    {
        userPreferenceRepository = unitOfWork.GetRepository<IUserPreferenceRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the profile preferences of a user
    /// </summary>
    /// <returns>True if the application has been initialized, False otherwise</returns>
    public async Task<ErrorOr<ProfilePreferencesDto>> Handle(GetProfilePreferencesQuery request, CancellationToken cancellationToken)
    {
        // if the repository reports an error, or there are no user preferences, there is an error
        var resultSelectUserPreferences = await userPreferenceRepository.GetByIdAsync(request.UserId.ToString());
        if (resultSelectUserPreferences.Error is null && resultSelectUserPreferences.Data is not null)
            return resultSelectUserPreferences.Data[0].Adapt<ProfilePreferencesDto>();
        else
            return Errors.DataAccess.GetUserPreferencesError;
    }
    #endregion
}