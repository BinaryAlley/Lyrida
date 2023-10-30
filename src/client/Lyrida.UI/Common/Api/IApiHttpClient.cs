#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Lyrida.UI.Common.DTO.FileSystem;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.UI.Common.Api;

/// <summary>
/// Interface for typed clients for API access
/// </summary>
/// <remarks>
/// Creation Date: 25th of July, 2023
/// </remarks>
public interface IApiHttpClient
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Sends a GET request to the specified <paramref name="endpoint"/> as an asynchronous operation and returns the result
    /// </summary>
    /// <param name="endpoint">The API endpoint where the request is being sent</param>
    /// <param name="token">The token used for authentication with the API</param>
    /// <param name="language">The language in which the API should respond</param>
    /// <param name="environment">The environment for which to make the POST request, in regard to filesystem operations</param>
    /// <returns>A string containing the result of the GET request</returns>
    Task<string> GetAsync(string endpoint, string? token = null, Language language = Language.English, EnvironmentType environment = EnvironmentType.LocalFileSystem, PlatformType platform = PlatformType.Unix);

    /// <summary>
    /// Sends a GET request to the specified <paramref name="endpoint"/> as an asynchronous operation and returns the binary result
    /// </summary>
    /// <param name="endpoint">The API endpoint where the request is being sent</param>
    /// <param name="token">The token used for authentication with the API</param>
    /// <param name="language">The language in which the API should respond</param>
    /// <param name="environment">The environment for which to make the POST request, in regard to filesystem operations</param>
    /// <returns>An object containing the content and its type</returns>
    Task<BlobDataDto> GetBlobAsync(string endpoint, string? token = null, Language language = Language.English, EnvironmentType environment = EnvironmentType.LocalFileSystem, PlatformType platform = PlatformType.Unix);

    /// <summary>
    /// Sends a DELETE request to the specified <paramref name="endpoint"/> as an asynchronous operation and returns the result
    /// </summary>
    /// <param name="endpoint">The API endpoint where the request is being sent</param>
    /// <param name="token">The token used for authentication with the API</param>
    /// <param name="language">The language in which the API should respond</param>
    /// <param name="environment">The environment for which to make the POST request, in regard to filesystem operations</param>
    /// <returns>A string containing the result of the DELETE request</returns>
    Task<string> DeleteAsync(string endpoint, string? token = null, Language language = Language.English, EnvironmentType environment = EnvironmentType.LocalFileSystem, PlatformType platform = PlatformType.Unix);

    /// <summary>
    /// Sends a PUT request to the specified <paramref name="endpoint"/> as an asynchronous operation and returns the result
    /// </summary>
    /// <param name="endpoint">The API endpoint where the request is being sent</param>
    /// <param name="data">The data to be serialized and send to the API</param>
    /// <param name="token">The token used for authentication with the API</param>
    /// <param name="language">The language in which the API should respond</param>
    /// <param name="environment">The environment for which to make the POST request, in regard to filesystem operations</param>
    /// <returns>A string containing the result of the PUT request</returns>
    Task<string> PutAsync<TDto>(string endpoint, TDto data, string? token = null, Language language = Language.English, EnvironmentType environment = EnvironmentType.LocalFileSystem, PlatformType platform = PlatformType.Unix);

    /// <summary>
    /// Sends a POST request to the specified <paramref name="endpoint"/> as an asynchronous operation and returns the result
    /// </summary>
    /// <param name="endpoint">The API endpoint where the request is being sent</param>
    /// <param name="data">The data to be serialized and sent to the API</param>
    /// <param name="token">The token used for authentication with the API</param>
    /// <param name="language">The language in which the API should respond</param>
    /// <param name="environment">The environment for which to make the POST request, in regard to filesystem operations</param>
    /// <returns>A string containing the result of the POST request</returns>
    Task<string> PostAsync<TDto>(string endpoint, TDto data, string? token = null, Language language = Language.English, EnvironmentType environment = EnvironmentType.LocalFileSystem, PlatformType platform = PlatformType.Unix);
    #endregion
}