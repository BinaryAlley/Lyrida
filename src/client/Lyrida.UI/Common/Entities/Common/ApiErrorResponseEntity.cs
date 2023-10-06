#region ========================================================================= USING =====================================================================================
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
#endregion

namespace Lyrida.UI.Common.Entities.Common;

/// <summary>
/// Deserialization DTO for the problem details returned by the remote API
/// </summary>
/// <remarks>
/// Creation Date: 26th of July, 2023
/// </remarks>
public class ApiErrorResponseEntity
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int Status { get; set; }
    public string? TraceId { get; set; }


    [JsonProperty("errors")]
    private object ErrorsObject
    {
        set
        {
            switch (value)
            {
                case JArray array:
                    Errors = array.ToObject<List<string>>();
                    break;
                case JObject obj:
                    ValidationErrors = obj.ToObject<Dictionary<string, List<string>>>();
                    break;
            }
        }
    }

    public List<string>? Errors { get; set; }
    public Dictionary<string, List<string>>? ValidationErrors { get; set; }
    #endregion
}