﻿#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.Common.Entities.Common;
using Lyrida.DataAccess.Repositories.Common.Base;
using Lyrida.DataAccess.Repositories.Common.Actions;
using Lyrida.DataAccess.Common.Entities.Authentication;
#endregion

namespace Lyrida.DataAccess.Repositories.Users;

/// <summary>
/// User repository interface for the bridge-through between the generic storage medium and storage medium for Users
/// </summary>
/// <remarks>
/// Creation Date: 12th of July, 2023
/// </remarks>
public interface IUserRepository : IRepository<UserEntity>,
                                   IDeleteByIdRepositoryAction,
                                   IInsertRepositoryAction<UserEntity>,
                                   IUpdateRepositoryAction<UserEntity>,
                                   IGetAllRepositoryAction<UserEntity>,
                                   IGetByIdRepositoryAction<UserEntity>
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the user identified by <paramref name="email"/> from the storage medium
    /// </summary>
    /// <param name="email">The email of the user to get</param>
    /// <returns>A user identified by <paramref name="email"/>, wrapped in a generic API container of type <see cref="ApiResponse{UserEntity}"/></returns>
    Task<ApiResponse<UserEntity>> GetByEmailAsync(string email);

    /// <summary>
    /// Gets the user with the <paramref name="token"/> registration token from the storage medium
    /// </summary>
    /// <param name="token">The registration validation token of the user to get</param>
    /// <returns>A user with the <paramref name="token"/> registration token, wrapped in a generic API container of type <see cref="ApiResponse{UserEntity}"/></returns>
    Task<ApiResponse<UserEntity>> GetByValidationTokenAsync(string token);

    /// <summary>
    /// Updates the password for <paramref name="entity"/> in the storage medium
    /// </summary>
    /// <param name="entity">The user whose password will be updated</param>
    /// <returns>The result of updating the password of <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
    Task<ApiResponse> ChangePasswordAsync(UserEntity entity);
    #endregion
}