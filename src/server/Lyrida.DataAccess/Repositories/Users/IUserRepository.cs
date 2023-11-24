#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.Repositories.Common.Base;
using Lyrida.DataAccess.Repositories.Common.Actions;
using Lyrida.DataAccess.Common.DTO.Authentication;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Users;

/// <summary>
/// User repository interface for the bridge-through between the generic storage medium and storage medium for Users
/// </summary>
/// <remarks>
/// Creation Date: 12th of July, 2023
/// </remarks>
public interface IUserRepository : IRepository<UserDto>,
                                   IDeleteByIdRepositoryAction,
                                   IInsertRepositoryAction<UserDto>,
                                   IUpdateRepositoryAction<UserDto>,
                                   IGetAllRepositoryAction<UserDto>,
                                   IGetByIdRepositoryAction<UserDto>
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the user identified by <paramref name="username"/> from the storage medium.
    /// </summary>
    /// <param name="username">The username of the user to get.</param>
    /// <returns>A user identified by <paramref name="username"/>, wrapped in a generic API container of type <see cref="ApiResponse{UserDto}"/>.</returns>
    Task<ApiResponse<UserDto>> GetByUsernameAsync(string username);

    /// <summary>
    /// Gets the user with the <paramref name="token"/> registration token from the storage medium.
    /// </summary>
    /// <param name="token">The registration validation token of the user to get.</param>
    /// <returns>A user with the <paramref name="token"/> registration token, wrapped in a generic API container of type <see cref="ApiResponse{UserDto}"/>.</returns>
    Task<ApiResponse<UserDto>> GetByValidationTokenAsync(string token);

    /// <summary>
    /// Updates the password for <paramref name="data"/> in the storage medium.
    /// </summary>
    /// <param name="data">The user whose password will be updated.</param>
    /// <returns>The result of updating the password of <paramref name="data"/>, wrapped in a generic API container of type <see cref="ApiResponse"/>.</returns>
    Task<ApiResponse> ChangePasswordAsync(UserDto data);
    #endregion
}