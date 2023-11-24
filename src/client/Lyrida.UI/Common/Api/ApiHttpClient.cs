#region ========================================================================= USING =====================================================================================
using System;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Lyrida.UI.Common.Exceptions;
using Lyrida.UI.Common.DTO.Common;
using Lyrida.UI.Common.DTO.FileSystem;
using Lyrida.Infrastructure.Common.Enums;
using Microsoft.Extensions.Configuration;
#endregion

namespace Lyrida.UI.Common.Api;

/// <summary>
/// Typed client for API access
/// </summary>
/// <remarks>
/// Creation Date: 25th of July, 2023
/// </remarks>
public class ApiHttpClient : IApiHttpClient
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly HttpClient httpClient;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="httpClient">Injected HttpClient for interacting with the API</param>
    public ApiHttpClient(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        httpClient.BaseAddress = new Uri(configuration.GetValue<string>("ServerPath") + ":" + configuration.GetValue<int>("ServerPort"));
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Sends a POST request to the specified <paramref name="endpoint"/> as an asynchronous operation and returns the result
    /// </summary>
    /// <param name="endpoint">The API endpoint where the request is being sent</param>
    /// <param name="data">The data to be serialized and sent to the API</param>
    /// <param name="token">The token used for authentication with the API</param>
    /// <param name="language">The language in which the API should respond</param>
    /// <param name="environment">The environment for which to make the POST request, in regard to filesystem operations</param>
    /// <returns>A string containing the result of the POST request</returns>
    public async Task<string> PostAsync<TDto>(string endpoint, TDto data, string? token = null, Language language = Language.English, EnvironmentType environment = EnvironmentType.LocalFileSystem, PlatformType platform = PlatformType.Unix)
    {
        if (!string.IsNullOrEmpty(token))
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Add("Accept-Language", language.ToString());
        httpClient.DefaultRequestHeaders.Add("X-Environment-Type", environment.ToString());
        httpClient.DefaultRequestHeaders.Add("X-Platform-Type", platform.ToString());
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(endpoint, content);
        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new ApiException(JsonConvert.DeserializeObject<ApiErrorResponseDto>(responseContent), response.StatusCode);
        return responseContent;
    }

    /// <summary>
    /// Sends a PUT request to the specified <paramref name="endpoint"/> as an asynchronous operation and returns the result
    /// </summary>
    /// <param name="endpoint">The API endpoint where the request is being sent</param>
    /// <param name="data">The data to be serialized and send to the API</param>
    /// <param name="token">The token used for authentication with the API</param>
    /// <param name="language">The language in which the API should respond</param>
    /// <param name="environment">The environment for which to make the POST request, in regard to filesystem operations</param>
    /// <returns>A string containing the result of the PUT request</returns>
    public async Task<string> PutAsync<TDto>(string endpoint, TDto data, string? token = null, Language language = Language.English, EnvironmentType environment = EnvironmentType.LocalFileSystem, PlatformType platform = PlatformType.Unix)
    {
        if (!string.IsNullOrEmpty(token))
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Add("Accept-Language", language.ToString());
        httpClient.DefaultRequestHeaders.Add("X-Environment-Type", environment.ToString());
        httpClient.DefaultRequestHeaders.Add("X-Platform-Type", platform.ToString());
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync(endpoint, content);
        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new ApiException(JsonConvert.DeserializeObject<ApiErrorResponseDto>(responseContent), response.StatusCode);
        return responseContent;
    }

    /// <summary>
    /// Sends a GET request to the specified <paramref name="endpoint"/> as an asynchronous operation and returns the result
    /// </summary>
    /// <param name="endpoint">The API endpoint where the request is being sent</param>
    /// <param name="token">The token used for authentication with the API</param>
    /// <param name="language">The language in which the API should respond</param>
    /// <param name="environment">The environment for which to make the POST request, in regard to filesystem operations</param>
    /// <returns>A string containing the result of the GET request</returns>
    public async Task<string> GetAsync(string endpoint, string? token = null, Language language = Language.English, EnvironmentType environment = EnvironmentType.LocalFileSystem, PlatformType platform = PlatformType.Unix)
    {
        if (!string.IsNullOrEmpty(token))
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Add("Accept-Language", language.ToString());
        httpClient.DefaultRequestHeaders.Add("X-Environment-Type", environment.ToString());
        httpClient.DefaultRequestHeaders.Add("X-Platform-Type", platform.ToString());
        var response = await httpClient.GetAsync(endpoint);
        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new ApiException(JsonConvert.DeserializeObject<ApiErrorResponseDto>(responseContent), response.StatusCode);
        return responseContent;
    }

    /// <summary>
    /// Sends a GET request to the specified <paramref name="endpoint"/> as an asynchronous operation and returns the binary result
    /// </summary>
    /// <param name="endpoint">The API endpoint where the request is being sent</param>
    /// <param name="token">The token used for authentication with the API</param>
    /// <param name="language">The language in which the API should respond</param>
    /// <param name="environment">The environment for which to make the POST request, in regard to filesystem operations</param>
    /// <returns>An object containing the content and its type</returns>
    public async Task<BlobDataDto> GetBlobAsync(string endpoint, string? token = null, Language language = Language.English, EnvironmentType environment = EnvironmentType.LocalFileSystem, PlatformType platform = PlatformType.Unix)
    {
        if (!string.IsNullOrEmpty(token))
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Add("Accept-Language", language.ToString());
        httpClient.DefaultRequestHeaders.Add("X-Environment-Type", environment.ToString());
        httpClient.DefaultRequestHeaders.Add("X-Platform-Type", platform.ToString());
        var response = await httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            // read the content as a string, for error messages
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new ApiException(JsonConvert.DeserializeObject<ApiErrorResponseDto>(errorResponse), response.StatusCode);
        }
        // if the response is successful, read it as a byte array
        byte[] responseContent = await response.Content.ReadAsByteArrayAsync();
        string contentType = response.Content.Headers.ContentType!.ToString();
        return new BlobDataDto() { Data = responseContent, ContentType = contentType };
    }

    /// <summary>
    /// Sends a DELETE request to the specified <paramref name="endpoint"/> as an asynchronous operation and returns the result
    /// </summary>
    /// <param name="endpoint">The API endpoint where the request is being sent</param>
    /// <param name="token">The token used for authentication with the API</param>
    /// <param name="language">The language in which the API should respond</param>
    /// <param name="environment">The environment for which to make the POST request, in regard to filesystem operations</param>
    /// <returns>A string containing the result of the DELETE request</returns>
    public async Task<string> DeleteAsync(string endpoint, string? token = null, Language language = Language.English, EnvironmentType environment = EnvironmentType.LocalFileSystem, PlatformType platform = PlatformType.Unix)
    {
        if (!string.IsNullOrEmpty(token))
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Add("Accept-Language", language.ToString());
        httpClient.DefaultRequestHeaders.Add("X-Environment-Type", environment.ToString());
        httpClient.DefaultRequestHeaders.Add("X-Platform-Type", platform.ToString());
        var response = await httpClient.DeleteAsync(endpoint);
        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new ApiException(JsonConvert.DeserializeObject<ApiErrorResponseDto>(responseContent), response.StatusCode);
        return responseContent;
    }
    #endregion
}