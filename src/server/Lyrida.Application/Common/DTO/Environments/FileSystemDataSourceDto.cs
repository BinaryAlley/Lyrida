#region ========================================================================= USING =====================================================================================
using System;
using Mapster;
#endregion

namespace Lyrida.Application.Common.DTO.Environments;

/// <summary>
/// User environments data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 22nd of November, 2023
/// </remarks>
public class FileSystemDataSourceDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public int UserId { get; set; }
    public Guid EnvironmentId { get; set; }
    public string Title { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string InitialPath { get; set; } = null!;
    public string PlatformType { get; set; } = null!;
    public string? Url { get; set; }
    public int? Port { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool? PassiveMode { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Customized ToString() method
    /// </summary>
    /// <returns>Custom string value showing relevant data for current class</returns>
    public override string ToString()
    {
        return Id + " :: " + UserId;
    }

    /// <summary>
    /// Maps between this DTO and the coresponding persistance DTO
    /// </summary>
    /// <returns>A data storage DTO representation of this DTO</returns>
    public DataAccess.Common.DTO.Environments.FileSystemDataSourceDto ToStorageDto()
    {
        return this.Adapt<DataAccess.Common.DTO.Environments.FileSystemDataSourceDto>();
    }
    #endregion
}