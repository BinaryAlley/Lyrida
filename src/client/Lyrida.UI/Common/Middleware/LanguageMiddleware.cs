#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Localization;
#endregion

namespace Lyrida.UI.Common.Middleware;

/// <summary>
/// Middleware for handling the language settings in the incoming request
/// </summary>
/// <remarks>
/// Creation Date: 13th of June, 2023
/// </remarks>
public class LanguageMiddleware
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    /// <summary>
    /// The next delegate in the middleware pipeline
    /// </summary>
    private readonly RequestDelegate next;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Constructor that takes the next delegate in the middleware pipeline.
    /// </summary>
    /// <param name="next">The next delegate in the middleware pipeline</param>
    public LanguageMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Middleware pipeline method to check and set the language preference based on the 'Language' cookie
    /// </summary>
    /// <param name="context">The current HttpContext for the middleware pipeline</param>
    /// <param name="translationService">The translation service used to set the language preference</param>
    /// <returns>A Task representing the completion of the middleware operation</returns>
    public async Task InvokeAsync(HttpContext context, ITranslationService translationService)
    {
        string? language = context.Request.Cookies["Language"];
        if (string.IsNullOrEmpty(language))
            language = "en"; // default language
        translationService.Language = language == "en" ? Language.English : language == "ro" ? Language.Romanian : Language.German;
        await next.Invoke(context);
    }
    #endregion
}