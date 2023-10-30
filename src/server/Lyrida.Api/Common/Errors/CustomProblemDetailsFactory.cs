﻿#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using System.Linq;
using System.Diagnostics;
using Lyrida.Api.Common.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Infrastructure;
#endregion

namespace Lyrida.Api.Common.Errors;

/// <summary>
/// Problem details abstract factory which adds custom properties to the default implementation provided by Microsoft
/// </summary>
/// <remarks>
/// Creation Date: 14th of July, 2023
/// </remarks>
public class CustomProblemDetailsFactory : ProblemDetailsFactory
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ApiBehaviorOptions options;
    private readonly ITranslationService translationService;
    private readonly Action<ProblemDetailsContext>? configure;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="options">Injected service for retrieving ApiBehaviorOptions</param>
    /// <param name="problemDetailsOptions">Injected service for retrieving ProblemDetailsOptions</param>
    /// <exception cref="ArgumentNullException">Thrown when the value of options (if any) is null</exception>
    public CustomProblemDetailsFactory(IOptions<ApiBehaviorOptions> options, ITranslationService translationService, IOptions<ProblemDetailsOptions>? problemDetailsOptions = null)
    {
        this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        this.translationService = translationService;
        configure = problemDetailsOptions?.Value?.CustomizeProblemDetails;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates a <see cref="ProblemDetails"/> instance that configures defaults based on values specified in <see cref="ApiBehaviorOptions"/>.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/>.</param>
    /// <param name="statusCode">The value for <see cref="ProblemDetails.Status"/>.</param>
    /// <param name="title">The value for <see cref="ProblemDetails.Title"/>.</param>
    /// <param name="type">The value for <see cref="ProblemDetails.Type"/>.</param>
    /// <param name="detail">The value for <see cref="ProblemDetails.Detail"/>.</param>
    /// <param name="instance">The value for <see cref="ProblemDetails.Instance"/>.</param>
    /// <returns>The <see cref="ProblemDetails"/> instance.</returns>
    public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null, string? type = null, 
        string? detail = null, string? instance = null)
    {
        if (httpContext.Items.ContainsKey(HttpContextItemKeys.LANGUAGE))
        {
            var language = (Language)Enum.Parse(typeof(Language), httpContext.Items[HttpContextItemKeys.LANGUAGE]?.ToString()!);
            translationService.Language = language;
        }
        statusCode ??= 500;
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = type,
            Detail = detail,
            Instance = instance,
        };
        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);
        return problemDetails;
    }

    /// <summary>
    /// Creates a <see cref="ValidationProblemDetails"/> instance that configures defaults based on values specified in <see cref="ApiBehaviorOptions"/>.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext" />.</param>
    /// <param name="modelStateDictionary">The <see cref="ModelStateDictionary"/>.</param>
    /// <param name="statusCode">The value for <see cref="ProblemDetails.Status"/>.</param>
    /// <param name="title">The value for <see cref="ProblemDetails.Title"/>.</param>
    /// <param name="type">The value for <see cref="ProblemDetails.Type"/>.</param>
    /// <param name="detail">The value for <see cref="ProblemDetails.Detail"/>.</param>
    /// <param name="instance">The value for <see cref="ProblemDetails.Instance"/>.</param>
    /// <returns>The <see cref="ValidationProblemDetails"/> instance.</returns>
    public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null,
        string? title = null, string? type = null, string? detail = null, string? instance = null)
    {
        if (httpContext.Items.ContainsKey(HttpContextItemKeys.LANGUAGE))
        {
            var language = (Language)Enum.Parse(typeof(Language), httpContext.Items[HttpContextItemKeys.LANGUAGE]?.ToString()!);
            translationService.Language = language;
        }
        ArgumentNullException.ThrowIfNull(modelStateDictionary);
        statusCode ??= 400;
        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Status = statusCode,
            Type = type,
            Detail = detail,
            Instance = instance,
        };
        if (title != null)            
            problemDetails.Title = title; // for validation problem details, don't overwrite the default title with null
        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);
        return problemDetails;
    }

    /// <summary>
    /// Creates a <see cref="ValidationProblemDetails"/> instance that configures defaults based on values specified in <see cref="ApiBehaviorOptions"/>.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext" />.</param>
    /// <param name="statusCode">The value for <see cref="ProblemDetails.Status"/>.</param>
    /// <param name="problemDetails">The value for <see cref="ProblemDetails"/>.</param>
    /// <returns>The <see cref="ValidationProblemDetails"/> instance.</returns>
    private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
    {
        // the language might have been sent along with the HTTP request, and intercepted by the language middleware; if so, use it!
        if (httpContext.Items.ContainsKey(HttpContextItemKeys.LANGUAGE))
        {
            var language = (Language)Enum.Parse(typeof(Language), httpContext.Items[HttpContextItemKeys.LANGUAGE]?.ToString()!);
            translationService.Language = language;
        }
        problemDetails.Status ??= statusCode;
        if (options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }
        var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        if (traceId != null)
            problemDetails.Extensions["traceId"] = traceId;
        // get any errors that might be sent by ApiController base class' Problem() method
        var errors = httpContext?.Items[HttpContextItemKeys.ERRORS] as List<Error>;
        // add any extra custom properties
        if (errors is not null)
        {
            problemDetails.Extensions.Add("errors", errors.Select(e =>
            {
                if (Enum.TryParse<Terms>(e.Code, out var term)) // if there is a translation available for the error code, use it
                    return translationService.Translate(term);
                else
                    return e.Code;
            }));
        }
        //problemDetails.Extensions.Add("errorCodes", errors.Select(e => e.Type.ToString()));
        configure?.Invoke(new() { HttpContext = httpContext!, ProblemDetails = problemDetails });
    }
    #endregion
}