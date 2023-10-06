#region ========================================================================= USING =====================================================================================
using System;
using Lyrida.UI.Common.Api;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lyrida.Infrastructure.Localization;
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
    public IActionResult Index()
    {
        return View();
    }
    #endregion
}