#region ========================================================================= USING =====================================================================================
using ErrorOr;
using System.Linq;
using Lyrida.Api.Common.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Lyrida.Infrastructure.Localization;
using Lyrida.Infrastructure.Common.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
#endregion

namespace Lyrida.Api.Controllers;

/// <summary>
/// User repository interface for the bridge-through between the generic storage medium and storage medium for Users
/// </summary>
/// <remarks>
/// Creation Date: 17th of July, 2023
/// </remarks>
[Authorize]
[ApiController]
public class ApiController : ControllerBase
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ITranslationService translationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="translationService">Injected service for localization</param>
    public ApiController(ITranslationService translationService)
    {
        this.translationService = translationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Handles controllers errors
    /// </summary>
    /// <param name="errors">The error to handle</param>
    protected internal IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0) // if there are no custom errors given, return a generic problem
            return Problem();
        // try to make the problem type more specific
        if (errors.All(error => error.Type == ErrorType.Validation))
            return ValidationProblem(errors);        
        // here we could handle custom error types, for instance: if (errors.All(error => error.NumericType == 23)), where 23 could be "domain errors"

        // share the list of errors within the scope of this HTTP request
        HttpContext.Items[HttpContextItemKeys.ERRORS] = errors;
        // get a HTTP error status code for the selected domain error type
        return Problem(errors.First());
    }

    /// <summary>
    /// Converts an <see cref="Error"/> to a standard problem response
    /// </summary>
    /// <param name="error">The error to convert</param>
    protected internal IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            // TODO: correct
            // ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Failure => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,

        };
        var errorTerm = error.Type switch
        {
            ErrorType.Conflict => Terms.Conflict,
            ErrorType.NotFound => Terms.NotFound,
            ErrorType.Validation => Terms.BadRequest,
            // TODO: correct
            //ErrorType.Unauthorized => Terms.Unauthorized,
            ErrorType.Failure => Terms.Forbidden,
            _ => Terms.InternalServerError,

        };
        // return the HTTP error status and the translated domain error message
        //return Problem(statusCode: statusCode);
        return Problem(statusCode: statusCode, title: translationService.Translate(errorTerm));
    }

    /// <summary>
    /// Converts a list of <see cref="Error"/> into a standard validation problem response
    /// </summary>
    /// <param name="errors">The list of errors to convert</param>
    protected internal IActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();
        // TODO: correct
        //foreach (var error in errors)
        //    modelStateDictionary.AddModelError(error.Term.ToString(), translationService.Translate(error.Term));
        return ValidationProblem(modelStateDictionary: modelStateDictionary, title: translationService.Translate(Terms.ValidationError));
    }
    #endregion
}