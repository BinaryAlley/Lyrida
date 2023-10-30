#region ========================================================================= USING =====================================================================================
using Newtonsoft.Json;
using Lyrida.UI.Common.Api;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Localization;
using Lyrida.UI.Common.DTO.Authorization;
#endregion

namespace Lyrida.UI.Controllers;

/// <summary>
/// Controller for permissions and roles
/// </summary>
/// <remarks>
/// Creation Date: 11th of August, 2023
/// </remarks>
[Authorize]
[Route("[controller]")]
public class PermissionsController : Controller
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IApiHttpClient apiHttpClient;
    private readonly ITranslationService translationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    /// <param name="translationService">Injected service for translations</param>
    /// <param name="httpClient">Injected service for interactions with the API</param>
    public PermissionsController(IApiHttpClient httpClient, ITranslationService translationService)
    {
        apiHttpClient = httpClient;
        this.translationService = translationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Displays the view needed to add, modify or delete roles
    /// </summary>
    [HttpGet("AddRoles")]
    public async Task<IActionResult> AddRoles()
    {
        // get the list of roles
        var response = await apiHttpClient.GetAsync("roles/", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        ViewData["roles"] = JsonConvert.DeserializeObject<RoleDto[]>(response);
        // get the list of user permissions
        response = await apiHttpClient.GetAsync("permissions/", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        ViewData["rolePermissions"] = JsonConvert.DeserializeObject<PermissionDto[]>(response);
        return PartialView();
    }

    /// <summary>
    /// Gets the list of roles
    /// </summary>
    [HttpGet("GetRoles")]
    public async Task<IActionResult> GetRoles()
    {
        var response = await apiHttpClient.GetAsync("roles/", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        return Json(new { success = true, roles = JsonConvert.DeserializeObject<RoleDto[]>(response) });
    }

    /// <summary>
    /// Gets the roles of a user identified by <paramref name="userId"/>
    /// </summary>
    /// <param name="userId">The id of the user for which to get the list of roles</param>
    [HttpGet("GetRolesByUserId/{userId}")]
    public async Task<IActionResult> GetRolesByUserId(int userId)
    {
        var response = await apiHttpClient.GetAsync($"users/{userId}/roles", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        return Json(new { success = true, userRoles = JsonConvert.DeserializeObject<RoleDto[]>(response) });
    }

    /// <summary>
    /// Gets the permissions of a role identified by <paramref name="roleId"/>
    /// </summary>
    /// <param name="roleId">The id of the role for which to get the list of permissions</param>
    [HttpGet("GetPermissionsByRoleId/{roleId}")]
    public async Task<IActionResult> GetPermissionsByRoleId(int roleId)
    {
        var response = await apiHttpClient.GetAsync($"roles/{roleId}/permissions", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        return Json(new { success = true, rolePermissions = JsonConvert.DeserializeObject<PermissionDto[]>(response) });
    }

    /// <summary>
    /// Gets the permissions of a user identified by <paramref name="userId"/>
    /// </summary>
    /// <param name="userId">The id of the user for which to get the list of permissions</param>
    [HttpGet("GetPermissionsByUserId/{userId}")]
    public async Task<IActionResult> GetPermissionsByUserId(int userId)
    {
        var response = await apiHttpClient.GetAsync($"users/{userId}/permissions", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        return Json(new { success = true, userPermissions = JsonConvert.DeserializeObject<UserPermissionDto[]>(response) });
    }

    /// <summary>
    /// Creates a role with the specified name and associated permissions.
    /// </summary>
    /// <param name="roleName">The name of the role to be created</param>
    /// <param name="rolePermissions">The list of permission IDs associated with the role</param>
    [HttpPost("CreateRole")]
    public async Task<IActionResult> CreateRole(string roleName, List<int> rolePermissions)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return Json(new { errorMessage = translationService.Translate(Terms.RoleNameCannotBeEmpty) });
        if (rolePermissions?.Count == 0)
            return Json(new { errorMessage = translationService.Translate(Terms.RoleMustHavePermissions) });
        await apiHttpClient.PostAsync("roles/", new { roleName, permissions = rolePermissions }, HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        return Json(new { success = true });
    }

    /// <summary>
    /// Updates a role identified by <paramref name="rolesRoleId"/> and its associated permissions.
    /// </summary>
    /// <param name="roleName">The new name of the role to be updated</param>
    /// <param name="rolePermissions">The list of permission IDs associated with the role</param>
    /// <param name="rolesRoleId">The Id of the role to update</param>
    [HttpPost("UpdateRole")]
    public async Task<IActionResult> UpdateRole(string roleName, List<int> rolePermissions, string rolesRoleId)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return Json(new { errorMessage = translationService.Translate(Terms.RoleNameCannotBeEmpty) });
        if (rolePermissions?.Count == 0)
            return Json(new { errorMessage = translationService.Translate(Terms.RoleMustHavePermissions) });
        await apiHttpClient.PutAsync($"roles/{rolesRoleId}", new { roleName, permissions = rolePermissions }, 
            HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        return Json(new { success = true });
    }

    /// <summary>
    /// Deletes a role identified by <paramref name="roleId"/>
    /// </summary>
    /// <param name="roleId">The id of the role to be deleted</param>
    [HttpPost("DeleteRole")]
    public async Task<IActionResult> DeleteRole(int roleId)
    {
        await apiHttpClient.DeleteAsync($"roles/{roleId}", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        return Json(new { success = true });
    }
    #endregion
}