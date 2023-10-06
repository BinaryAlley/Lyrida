#region ========================================================================= USING =====================================================================================
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
#endregion

namespace Lyrida.Api.Controllers;

/// <summary>
/// Controller that receives errors from the errors middleware and returns the problem details
/// </summary>
/// <remarks>
/// Creation Date: 14th of July, 2023
/// </remarks>
public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        //Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        //return Problem(title: exception?.Message, statusCode: 400);
        return Problem();
    }
}