namespace Lyrida.DataAccess.StorageAccess;

/// <summary>
/// Interface for storage DTOs, enforces Id
/// </summary>
/// <remarks>
/// Creation Date: 12th of June, 2021
/// </remarks>
public interface IStorageDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    int Id { get; set; }
    #endregion
}