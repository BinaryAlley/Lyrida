#region ========================================================================= USING =====================================================================================
using System;
using Newtonsoft.Json;
using System.Net.Http;
using Lyrida.UI.Common.Api;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Lyrida.UI.Common.Exceptions;
using Lyrida.UI.Common.DTO.FileSystem;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Localization;
using Microsoft.AspNetCore.Authorization;
using Lyrida.UI.Common.DTO.Configuration;
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
        try
        {
            string response = await apiHttpClient.GetAsync($"account/getPreferences", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
            ProfilePreferencesDto? profilePreferences = JsonConvert.DeserializeObject<ProfilePreferencesDto>(response);
            response = await apiHttpClient.GetAsync($"pages", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
            PageDto[]? pages = JsonConvert.DeserializeObject<PageDto[]>(response);
            response = await apiHttpClient.GetAsync($"environments", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
            FileSystemDataSourceDto[]? environments = JsonConvert.DeserializeObject<FileSystemDataSourceDto[]>(response);

            ViewData["Preferences"] = profilePreferences;
            ViewData["InitialPages"] = pages;
            ViewData["Environments"] = environments;
            return View("Index");
        }
        catch (HttpRequestException)
        {
            ViewData["error"] = translationService.Translate(Terms.TheServerDidNotRespond);
            return View("Index");
        }
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
    /// Adds or updates a file system data source.
    /// </summary>
    /// <param name="dataSource">The file system data source to add or update.</param>
    [HttpPost("AddDataSource")]
    public async Task<IActionResult> AddDataSource([FromBody] FileSystemDataSourceDto dataSource)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        var response = await apiHttpClient.PostAsync($"environments", dataSource, HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform);
        return Json(new { success = true, message = translationService.Translate(Terms.DataSourceSaved) });
    }

    /// <summary>
    /// Creates a new directory.
    /// </summary>
    /// <param name="directory">DTO containing the path where the new directory will be created, and the name of the new directory.</param>
    [HttpPost("CreateDirectory")]
    public async Task<IActionResult> CreateDirectory([FromBody] CreateDirectoryRequestDto directory)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        // first, make sure the new path is a valid one
        if ((await apiHttpClient.GetAsync($"paths/validate?path={Uri.EscapeDataString(directory.Path + directory.Name)}", HttpContext.Items["UserToken"]?.ToString(),
           translationService.Language, environment, platform)) == "true")
        {
            var response = await apiHttpClient.PostAsync($"directories", directory, HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform);
            return Json(new { success = true, message = translationService.Translate(Terms.DirectoryCreated), directory = JsonConvert.DeserializeObject<DirectoryDto>(response) });
        }
        else // creating a new directory with the provided name, at the provided location, did not result in a valid path
            return Json(new { success = false, message = translationService.Translate(Terms.InvalidPath) });
    }

    /// <summary>
    /// Renames a filesystem element.
    /// </summary>
    /// <param name="element">DTO containing the path and the new name of the element to be renamed.</param>
    [HttpPost("RenameElement")]
    public async Task<IActionResult> RenameElement([FromBody] RenameFileSystemItemRequestDto element)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        // first, make sure the new path is a valid one
        if ((await apiHttpClient.GetAsync($"paths/validate?path={Uri.EscapeDataString(element.Path + element.Name)}", HttpContext.Items["UserToken"]?.ToString(),
           translationService.Language, environment, platform)) == "true")
        {
            var response = await apiHttpClient.PutAsync(element.IsFile ? $"files" : $"directories", element, HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform);
            return Json(new { success = true, message = translationService.Translate(element.IsFile ? Terms.FileRenamed : Terms.DirectoryRenamed), directory = JsonConvert.DeserializeObject<DirectoryDto>(response) });
        }
        else // creating a new directory with the provided name, at the provided location, did not result in a valid path
            return Json(new { success = false, message = translationService.Translate(Terms.InvalidPath) });
    }

    /// <summary>
    /// Copies a file.
    /// </summary>
    /// <param name="element">DTO containing the path of the file to be copied, and the path where it will be copied.</param>
    [HttpPost("CopyFile")]
    public async Task<IActionResult> CopyFile([FromBody] PasteFileSystemItemRequestDto element)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        // first, make sure the new path is a valid one
        if ((await apiHttpClient.GetAsync($"paths/validate?path={Uri.EscapeDataString(element.SourcePath!)}", HttpContext.Items["UserToken"]?.ToString(),
           translationService.Language, environment, platform)) == "true" && (await apiHttpClient.GetAsync($"paths/validate?path={Uri.EscapeDataString(element.DestinationPath!)}", 
           HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform)) == "true")
        {
            try
            {
                var response = await apiHttpClient.PostAsync($"files/copy", element, HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform);
                return Json(new { success = true, message = translationService.Translate(Terms.Copied), file = JsonConvert.DeserializeObject<FileDto>(response) });

            }
            catch (ApiException ex)
            {
                if (ex?.Error?.Errors?.Count > 0 && ex.Error.Status == 403 && ex.Error.Errors[0] == translationService.Translate(Terms.FileAlreadyExistsError))
                    return Json(new
                    {
                        success = false,
                        errorMessage = "file exists",
                        title = element.FileName + " - " + translationService.Translate(Terms.FileAlreadyExistsError) + 
                                    Environment.NewLine + translationService.Translate(Terms.ChooseAnAction),
                        replaceText = translationService.Translate(Terms.ReplaceTheFile),
                        replaceAllText = translationService.Translate(Terms.ReplaceAllFiles),
                        skipText = translationService.Translate(Terms.SkipThisFile),
                        cancelText = translationService.Translate(Terms.Cancel),
                        keepText = translationService.Translate(Terms.KeepBothFiles),
                    });
                throw;
            }
        }
        else 
            return Json(new { success = false, message = translationService.Translate(Terms.InvalidPath) });
    }

    /// <summary>
    /// Copies a directory.
    /// </summary>
    /// <param name="element">DTO containing the path of the directory to be copied, and the path where it will be copied.</param>
    [HttpPost("CopyDirectory")]
    public async Task<IActionResult> CopyDirectory([FromBody] PasteFileSystemItemRequestDto element)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        // first, make sure the new path is a valid one
        if ((await apiHttpClient.GetAsync($"paths/validate?path={Uri.EscapeDataString(element.SourcePath!)}", HttpContext.Items["UserToken"]?.ToString(),
           translationService.Language, environment, platform)) == "true" && (await apiHttpClient.GetAsync($"paths/validate?path={Uri.EscapeDataString(element.DestinationPath!)}",
           HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform)) == "true")
        {
            var response = await apiHttpClient.PostAsync($"directories/copy", element, HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform);
            return Json(new { success = true, message = translationService.Translate(Terms.Copied), directories = JsonConvert.DeserializeObject<DirectoryDto[]>(response) });
        }
        else
            return Json(new { success = false, message = translationService.Translate(Terms.InvalidPath) });
    }

    /// <summary>
    /// Moves a file.
    /// </summary>
    /// <param name="element">DTO containing the path of the file to be moved, and the path where it will be moved.</param>
    [HttpPost("MoveFile")]
    public async Task<IActionResult> MoveFile([FromBody] PasteFileSystemItemRequestDto element)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        if ((await apiHttpClient.GetAsync($"paths/validate?path={Uri.EscapeDataString(element.SourcePath!)}", HttpContext.Items["UserToken"]?.ToString(),
           translationService.Language, environment, platform)) == "true" && (await apiHttpClient.GetAsync($"paths/validate?path={Uri.EscapeDataString(element.DestinationPath!)}",
           HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform)) == "true")
        {
            var response = await apiHttpClient.PostAsync($"files/move", element, HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform);
            return Json(new { success = true, message = translationService.Translate(Terms.Moved), files = JsonConvert.DeserializeObject<FileDto[]>(response) });
        }
        else
            return Json(new { success = false, message = translationService.Translate(Terms.InvalidPath) });
    }

    /// <summary>
    /// Moves a directory.
    /// </summary>
    /// <param name="element">DTO containing the path of the directory to be moved, and the path where it will be moved.</param>
    [HttpPost("MoveDirectory")]
    public async Task<IActionResult> MoveDirectory([FromBody] PasteFileSystemItemRequestDto element)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        if ((await apiHttpClient.GetAsync($"paths/validate?path={Uri.EscapeDataString(element.SourcePath!)}", HttpContext.Items["UserToken"]?.ToString(),
           translationService.Language, environment, platform)) == "true" && (await apiHttpClient.GetAsync($"paths/validate?path={Uri.EscapeDataString(element.DestinationPath!)}",
           HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform)) == "true")
        {
            var response = await apiHttpClient.PostAsync($"directories/copy", element, HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform);
            return Json(new { success = true, message = translationService.Translate(Terms.Moved), directories = JsonConvert.DeserializeObject<DirectoryDto[]>(response) });
        }
        else
            return Json(new { success = false, message = translationService.Translate(Terms.InvalidPath) });
    }

    /// <summary>
    /// Deletes a directory located at <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path of the directory to be deleted.</param>
    [HttpDelete("DeleteDirectory")]
    public async Task<IActionResult> DeleteDirectory(string path)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        await apiHttpClient.DeleteAsync($"directories?path={Uri.EscapeDataString(path)}", HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform);
        return Json(new { success = true, message = translationService.Translate(Terms.Deleted) });
    }

    /// <summary>
    /// Deletes a file located at <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path of the file to be deleted.</param>
    [HttpDelete("DeleteFile")]
    public async Task<IActionResult> DeleteFile(string path)
    {
        PlatformType platform = (PlatformType)HttpContext.Items["Platform"]!;
        EnvironmentType environment = (EnvironmentType)HttpContext.Items["Environment"]!;
        await apiHttpClient.DeleteAsync($"files?path={Uri.EscapeDataString(path)}", HttpContext.Items["UserToken"]?.ToString(), translationService.Language, environment, platform);
        return Json(new { success = true, message = translationService.Translate(Terms.Deleted) });
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
        if (page.EnvironmentId != null)
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
    #endregion
}