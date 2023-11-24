#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Lyrida.DataAccess.Common.Enums;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.DTO.Common;
using Lyrida.DataAccess.Common.DTO.Environments;
#endregion

namespace Lyrida.DataAccess.Repositories.UserEnvironments;

/// <summary>
/// User environments repository for the bridge-through between the generic storage medium and storage medium for UserEnvironments
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
internal sealed class UserEnvironmentRepository : IUserEnvironmentRepository
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDataAccess dataAccess;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="dataAccess">Injected data access service.</param>
    public UserEnvironmentRepository(IDataAccess dataAccess)
    {
        this.dataAccess = dataAccess ?? throw new ArgumentException("Data access cannot be null!");
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Opens a transaction.
    /// </summary>
    public void OpenTransaction()
    {
        dataAccess.OpenTransaction();
    }

    /// <summary>
    /// Closes a transaction, rolls back changes if the transaction was faulty.
    /// </summary>
    public void CloseTransaction()
    {
        dataAccess.CloseTransaction();
    }

    /// <summary>
    /// Gets the user environment identified by <paramref name="environmentId"/> from the storage medium.
    /// </summary>
    /// <param name="environmentId">The Id of the user environment to get.</param>
    /// <returns>The user environment identified by <paramref name="environmentId"/>, wrapped in a generic API container of type <see cref="ApiResponse{FileSystemDataSourceDto}"/>.</returns>
    public async Task<ApiResponse<FileSystemDataSourceDto>> GetByIdAsync(string environmentId)
    {
        return await dataAccess.SelectAsync<FileSystemDataSourceDto>(DataContainers.UserEnvironments, new { environment_id = environmentId });
    }

    /// <summary>
    /// Gets the user environments of the user identified by <paramref name="userId"/> from the storage medium.
    /// </summary>
    /// <param name="userId">The Id of the user whose user environments to get.</param>
    /// <returns>The user environments of a user identified by <paramref name="userId"/>, wrapped in a generic API container of type <see cref="ApiResponse{FileSystemDataSourceDto}"/>.</returns>
    public async Task<ApiResponse<FileSystemDataSourceDto>> GetByUserIdAsync(string userId)
    {
        return await dataAccess.SelectAsync<FileSystemDataSourceDto>(DataContainers.UserEnvironments, new { user_id = userId });
    }

    /// <summary>
    /// Saves a user environment into the storage medium.
    /// </summary>
    /// <param name="data">The user environment to be saved.</param>
    /// <returns>The result of saving <paramref name="data"/>, wrapped in a generic API container of type <see cref="ApiResponse{FileSystemDataSourceDto}"/>.</returns>
    public async Task<ApiResponse<FileSystemDataSourceDto>> InsertAsync(FileSystemDataSourceDto data)
    {
        return await dataAccess.InsertAsync(DataContainers.UserEnvironments, data);
    }

    /// <summary>
    /// Deletes a user environment identified by <paramref name="environmentId"/> from the storage medium.
    /// </summary>
    /// <param name="environmentId">The id of the user environment to be deleted.</param>
    /// <returns>The result of deleting the user environment, wrapped in a generic API container of type <see cref="ApiResponse"/>.</returns>
    public async Task<ApiResponse> DeleteByIdAsync(string environmentId)
    {
        return await dataAccess.DeleteAsync(DataContainers.UserEnvironments, new { environment_id = environmentId });
    }

    /// <summary>
    /// Updates <paramref name="data"/> in the storage medium.
    /// </summary>
    /// <param name="data">The user environment that will be updated.</param>
    /// <returns>The result of updating <paramref name="data"/>, wrapped in a generic API container of type <see cref="ApiResponse"/>.</returns>
    public async Task<ApiResponse> UpdateAsync(FileSystemDataSourceDto data)
    {
        return await dataAccess.UpdateAsync(DataContainers.UserEnvironments, data, new { environment_id = data.EnvironmentId });
    }
    #endregion
}