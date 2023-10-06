#region ========================================================================= USING =====================================================================================
namespace Lyrida.Api.Common.Http;
#endregion

/// <summary>
/// Constants shared by HttpContext across a HTTP request (avoid magic strings)
/// </summary>
/// <remarks>
/// Creation Date: 17th of July, 2023
/// </remarks>
public static class HttpContextItemKeys
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    public const string ERRORS = "errors";
    public const string PLATFORM = "platform";
    public const string LANGUAGE = "language";
    public const string ENVIRONMENT = "environment";
    #endregion
}