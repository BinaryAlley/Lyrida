#region ========================================================================= USING =====================================================================================
using System;
using Newtonsoft.Json;
using Lyrida.UI.Common.Api;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Localization;
using Lyrida.UI.Common.Entities.FileSystem;
#endregion

namespace Lyrida.UI.Controllers;

/// <summary>
/// Controller for file system related operations
/// </summary>
/// <remarks>
/// Creation Date: 20th of September, 2023
/// </remarks>
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
    [HttpGet()]
    public IActionResult Index()
    {
        return View("Index");
    }

    [HttpGet("GetFiles")]
    public async Task<IActionResult> GetFiles(string path)
    {
        var response = await apiHttpClient.GetAsync($"files?path={path}", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        return Json(new { success = true, files = JsonConvert.DeserializeObject<FileEntity[]>(response) });
    }

    [HttpGet("GetDirectories")]
    public async Task<IActionResult> GetDirectories(string path)
    {
        var response = await apiHttpClient.GetAsync($"directories?path={path}", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        return Json(new { success = true, directories = JsonConvert.DeserializeObject<DirectoryEntity[]>(response) });
    }

    [HttpGet("GetThumbnail/{path}")]
    public async Task<IActionResult> GetThumbnail(string path)
    {
        var response = await apiHttpClient.GetBlobAsync($"thumbnails?path={Uri.EscapeDataString(path)}", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        return new ObjectResult(new
        {
            Data = Convert.ToBase64String(response.Data),
            MimeType = response.ContentType
        });
    }

    [HttpGet("CheckPath")]
    public async Task<IActionResult> CheckPath(string path)
    {
        var response = await apiHttpClient.GetAsync($"paths/validate?path={Uri.EscapeDataString(path)}", HttpContext.Items["UserToken"]?.ToString(), 
            translationService.Language, platform: PlatformType.Windows);
        return Json(new { success = response == "true" });
    }

    [HttpGet("ParsePath")]
    public async Task<IActionResult> ParsePath(string path)
    {
        var response = await apiHttpClient.GetAsync($"paths/parse?path={Uri.EscapeDataString(path)}", HttpContext.Items["UserToken"]?.ToString(), 
            translationService.Language, platform: PlatformType.Windows);
        return Json(new { success = true, pathSegments = JsonConvert.DeserializeObject<PathSegmentEntity[]>(response) });
    }
    #endregion
}