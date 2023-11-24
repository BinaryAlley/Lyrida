#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.Common.DTO.Common;
using Lyrida.DataAccess.Common.DTO.Environments;
using Lyrida.DataAccess.Repositories.Common.Base;
using Lyrida.DataAccess.Repositories.Common.Actions;
#endregion

namespace Lyrida.DataAccess.Repositories.UserEnvironments;

/// <summary>
/// User environment repository interface for the bridge-through between the generic storage medium and storage medium for UserEnvironments
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
public interface IUserEnvironmentRepository : IRepository<FileSystemDataSourceDto>,
                                              IInsertRepositoryAction<FileSystemDataSourceDto>,
                                              IUpdateRepositoryAction<FileSystemDataSourceDto>,
                                              IGetByIdRepositoryAction<FileSystemDataSourceDto>,
                                              IDeleteByIdRepositoryAction
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the user environments of the user identified by <paramref name="userId"/> from the storage medium.
    /// </summary>
    /// <param name="userId">The Id of the user whose user environments to get.</param>
    /// <returns>The user environments of a user identified by <paramref name="userId"/>, wrapped in a generic API container of type <see cref="ApiResponse{FileSystemDataSourceDto}"/>.</returns>
    Task<ApiResponse<FileSystemDataSourceDto>> GetByUserIdAsync(string userId);
    #endregion
}