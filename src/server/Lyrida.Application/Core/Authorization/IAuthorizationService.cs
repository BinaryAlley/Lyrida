#region ========================================================================= USING =====================================================================================
using ErrorOr;
using System.Threading.Tasks;
#endregion

namespace Lyrida.Application.Core.Authorization;

/// <summary>
/// Interface for authorization service
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public interface IAuthorizationService
{
    #region ==================================================================== PROPERTIES =================================================================================
    UserPermissions UserPermissions { get; }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the permissions of a user identified by <paramref name="userId"/>
    /// </summary>
    /// <param name="userId">The id of the user for which to get the permissions</param>
    /// <returns>True if the permissions were taken, False otherwise</returns>
    Task<ErrorOr<bool>> GetUserPermissionsAsync(int userId);
    #endregion
}