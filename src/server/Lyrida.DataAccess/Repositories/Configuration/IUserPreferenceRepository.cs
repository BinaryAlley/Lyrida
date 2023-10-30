#region ========================================================================= USING =====================================================================================
using Lyrida.DataAccess.Common.DTO.Configuration;
using Lyrida.DataAccess.Repositories.Common.Actions;
using Lyrida.DataAccess.Repositories.Common.Base;
#endregion

namespace Lyrida.DataAccess.Repositories.Configuration;

/// <summary>
/// User preferences repository interface for the bridge-through between the generic storage medium and storage medium for UserPreferences
/// </summary>
/// <remarks>
/// Creation Date: 25th of October, 2023
/// </remarks>
public interface IUserPreferenceRepository : IRepository<ProfilePreferencesDto>,
                                             IGetByIdRepositoryAction<ProfilePreferencesDto>,
                                             IInsertRepositoryAction<ProfilePreferencesDto>,
                                             IUpdateRepositoryAction<ProfilePreferencesDto>
{
}