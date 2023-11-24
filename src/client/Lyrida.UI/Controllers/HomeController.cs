#region ========================================================================= USING =====================================================================================
using Lyrida.UI.Common.Api;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lyrida.UI.Common.Exceptions;
using Lyrida.Infrastructure.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http;
using Lyrida.Infrastructure.Common.Enums;
using Newtonsoft.Json;
using Lyrida.UI.Common.DTO.FileSystem;
using System.Linq;
#endregion

namespace Lyrida.Client.Controllers;

/// <summary>
/// Home controller
/// </summary>
/// <remarks>
/// Creation Date: 19th of September, 2023
/// </remarks>
public class HomeController : Controller
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
    public HomeController(IApiHttpClient apiHttpClient, ITranslationService translationService)
    {
        this.apiHttpClient = apiHttpClient;
        this.translationService = translationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Checks if the API has been initialized, and if yes, displays the view for filesystem interactions. If not, displays the setup view.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        try
        {
            // check if the web app was initialized
            var response = await apiHttpClient.GetAsync("initialization/", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
            if (response == "true") // application was initialized
            {
                response = await apiHttpClient.GetAsync($"environments", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
                return View(JsonConvert.DeserializeObject<FileSystemDataSourceDto[]>(response)); 
            }
            else
                // TODO: should be:
                // return View("~/Views/Account/Register", new RegisterRequestDto());
                // but doesn't work because of an ASP.NET bug: https://github.com/dotnet/AspNetCore.Docs/issues/25157
                return RedirectToAction("Register", "Account");
        }
        catch (ApiException ex)
        {
            // if it got here, assume something bad, and sign out
            Response.Cookies.Delete("Token");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (ex.Error?.Errors?.First() == translationService.Translate(Terms.UninitializedDatabaseError))
                return RedirectToAction("Register", "Account");
            else
                return RedirectToAction("Login", "Account");
        }
        catch (HttpRequestException)
        {
            ViewData["error"] = translationService.Translate(Terms.TheServerDidNotRespond);
            return View();
        }
    }
    #endregion
}