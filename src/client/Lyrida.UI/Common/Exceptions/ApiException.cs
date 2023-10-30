#region ========================================================================= USING =====================================================================================
using System;
using System.Net;
using Lyrida.UI.Common.DTO.Common;
#endregion

namespace Lyrida.UI.Common.Exceptions;

/// <summary>
/// Custom exception for interaction with the remote API
/// </summary>
/// <remarks>
/// Creation Date: 26th of July, 2023
/// </remarks>
public class ApiException : Exception
{
    #region ==================================================================== PROPERTIES =================================================================================
    public HttpStatusCode HttpStatusCode { get; set; }
    public ApiErrorResponseDto? Error { get; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="error">The problem details object returned by the API</param>
    /// <param name="httpStatusCode">The HTTP status code returned by the API</param>
    public ApiException(ApiErrorResponseDto? error, HttpStatusCode httpStatusCode) : base(error?.Title)
    {
        Error = error;
        HttpStatusCode = httpStatusCode;
    }
    #endregion
}