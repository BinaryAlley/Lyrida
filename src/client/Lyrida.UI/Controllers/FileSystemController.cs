#region ========================================================================= USING =====================================================================================
using System;
using Newtonsoft.Json;
using Lyrida.UI.Common.Api;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lyrida.UI.Common.DTO.FileSystem;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Localization;
using Microsoft.AspNetCore.Authorization;
using Lyrida.UI.Common.DTO.Configuration;
using System.Collections.Generic;
#endregion

namespace Lyrida.UI.Controllers;

/// <summary>
/// Controller for file system related operations
/// </summary>
/// <remarks>
/// Creation Date: 20th of September, 2023
/// </remarks>
[Authorize]
[Route("[controller]")]
public class FileSystemController : Controller
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IApiHttpClient apiHttpClient;
    private readonly ITranslationService translationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="apiHttpClient">Injected service for interactions with the API</param>
    /// <param name="translationService">Injected service for translations</param>
    public FileSystemController(IApiHttpClient apiHttpClient, ITranslationService translationService)
    {
        this.apiHttpClient = apiHttpClient;
        this.translationService = translationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Displays the view for filesystem interactions.
    /// </summary>
    [HttpGet()]
    public async Task<IActionResult> Index()
    {
        string response = await apiHttpClient.GetAsync($"account/getPreferences", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        ProfilePreferencesDto? profilePreferences = JsonConvert.DeserializeObject<ProfilePreferencesDto>(response);
        response = await apiHttpClient.GetAsync($"pages", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        ViewData["Preferences"] = profilePreferences;
        ViewData["InitialPages"] = JsonConvert.DeserializeObject<List<PageDto>>(response);
        return View("Index");
    }

    /// <summary>
    /// Gets the files located at <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path for which to get the files.</param>
    [HttpGet("GetFiles")]
    public async Task<IActionResult> GetFiles(string path)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        var response = await apiHttpClient.GetAsync($"files?path={path}", HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform);
        return Json(new { success = true, files = JsonConvert.DeserializeObject<FileDto[]>(response) });
    }

    /// <summary>
    /// Gets the directories located at <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path for which to get the directories.</param>
    [HttpGet("GetDirectories")]
    public async Task<IActionResult> GetDirectories(string path)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        var response = await apiHttpClient.GetAsync($"directories?path={path}", HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform);
        return Json(new { success = true, directories = JsonConvert.DeserializeObject<DirectoryDto[]>(response) });
    }

    /// <summary>
    /// Gets the thumbnails of the files located at <paramref name="path"/>, with the specified <paramref name="quality"/>.
    /// </summary>
    /// <param name="path">The path for which to get the thumbnails.</param>
    /// <param name="quality">The quality used for the thumbnails.</param>
    [HttpGet("GetThumbnail")]
    public async Task<IActionResult> GetThumbnail(string path, int quality)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        var response = await apiHttpClient.GetBlobAsync($"thumbnails?path={Uri.EscapeDataString(path)}&quality={quality}", HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform);
        return new ObjectResult(new
        {
            Data = Convert.ToBase64String(response.Data),
            MimeType = response.ContentType
        });
    }

    /// <summary>
    /// Checks if <paramref name="path"/> is a valid path.
    /// </summary>
    /// <param name="path">The path to check.</param>
    [HttpGet("CheckPath")]
    public async Task<IActionResult> CheckPath(string path)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        var response = await apiHttpClient.GetAsync($"paths/validate?path={Uri.EscapeDataString(path)}", HttpContext.Items["UserToken"]?.ToString(), 
            translationService.Language, environment, platform);
        return Json(new { success = response == "true" });
    }

    /// <summary>
    /// Parses the path segments of <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path for which to parse the path segments.</param>
    [HttpGet("ParsePath")]
    public async Task<IActionResult> ParsePath(string path)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        var response = await apiHttpClient.GetAsync($"paths/parse?path={Uri.EscapeDataString(path)}", HttpContext.Items["UserToken"]?.ToString(), 
            translationService.Language, environment, platform);
        return Json(new { success = true, pathSegments = JsonConvert.DeserializeObject<PathSegmentDto[]>(response) });
    }

    /// <summary>
    /// Navigates one level up from <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path from which to navigate one level up.</param>
    [HttpGet("GoUpOneLevel")]
    public async Task<IActionResult> GoUpOneLevel(string path)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        var response = await apiHttpClient.GetAsync($"paths/goUpOneLevel?path={Uri.EscapeDataString(path)}", HttpContext.Items["UserToken"]?.ToString(),
            translationService.Language, environment, platform);
        return Json(new { success = true, pathSegments = JsonConvert.DeserializeObject<PathSegmentDto[]>(response) });
    }

    /// <summary>
    /// Stores a new user page
    /// </summary>
    /// <param name="page">The page to be stored</param>
    [HttpPost("AddPage")]
    public async Task<IActionResult> AddPage([FromBody] PageDto page)
    {
        await apiHttpClient.PostAsync($"pages", page, HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        return Json(new { success = true });
    }

    /// <summary>
    /// Removes an existing user page
    /// </summary>
    /// <param name="page">The page to be removed</param>
    [HttpPost("RemovePage")]
    public async Task<IActionResult> RemovePage([FromBody] string guid)
    {
        await apiHttpClient.DeleteAsync($"pages/{guid}", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        return Json(new { success = true });
    }

    /// <summary>
    /// Updates an existing user page
    /// </summary>
    /// <param name="page">The page to be updated</param>
    [HttpPost("UpdatePage")]
    public async Task<IActionResult> UpdatePage([FromBody] PageDto page)
    {
        await apiHttpClient.PutAsync($"pages", page, HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        return Json(new { success = true });
    }
    #endregion
}