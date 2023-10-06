#region ========================================================================= USING =====================================================================================
using Lyrida.DataAccess.StorageAccess;
#endregion

namespace Lyrida.DataAccess.Common.Entities.Common;

/// <summary>
/// Generic container for deserialization models and other data supplied by the storage medium
/// </summary>
/// <remarks>
/// Creation Date: 13th of January, 2020
/// </remarks>
/// <typeparam name="T">The type used for the Data array. It should implement <see cref="IStorageEntity"/></typeparam>
public sealed class ApiResponse<T> where T : IStorageEntity
{
    #region ==================================================================== PROPERTIES =================================================================================
    public T[]? Data { get; set; }
    public int Count { get; set; }
    public string? Error { get; set; }
    #endregion
}

/// <summary>
/// Generic container for deserialization models and other data supplied by the storage medium
/// </summary>
/// <remarks>
/// Creation Date: 13th of January, 2020
/// </remarks>
public sealed class ApiResponse
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Count { get; set; }
    public string? Error { get; set; }
    #endregion
}