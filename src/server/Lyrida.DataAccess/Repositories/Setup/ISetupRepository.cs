#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.DataAccess.Repositories.Common.Base;
using Lyrida.DataAccess.Common.DTO.Authentication;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Setup;

/// <summary>
/// Setup repository interface for the initial database setup
/// </summary>
/// <remarks>
/// Creation Date: 15th of August, 2023
/// </remarks>
public interface ISetupRepository : IRepository<UserDto>
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Initializes the database
    /// </summary>
    /// <returns>The result of setting up the database, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
    Task<ApiResponse> SetDatabase();
    #endregion
}