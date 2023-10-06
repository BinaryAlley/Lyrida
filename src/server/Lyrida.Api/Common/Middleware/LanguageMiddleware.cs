#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Lyrida.Api.Common.Http;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Localization;
#endregion

namespace Lyrida.Api.Common.Middleware;

/// <summary>
/// Middleware for setting the translation service language
/// </summary>
/// <remarks>
/// Creation Date: 27th of July, 2023
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
        var language = context.Request.Headers["Accept-Language"].ToString();
        if (!string.IsNullOrEmpty(language))
        {
            // TODO: repair in proper way:
            translationService.Language = Language.English;
            await next(context);
            return;
            // store the language in the context, so it can be used by members that had their translation service assigned before this middleware kicked in
            context.Items[HttpContextItemKeys.LANGUAGE] = Regex.Replace(language, ";q=[0-9\\.]+", ""); // discard any "priority" stuff from language
            translationService.Language = (Language)Enum.Parse(typeof(Language), language);
        }
        await next(context);
    }
    #endregion
}