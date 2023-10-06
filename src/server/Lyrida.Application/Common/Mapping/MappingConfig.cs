#region ========================================================================= USING =====================================================================================
using Mapster;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Application.Common.Entities.FileSystem;
#endregion

namespace Lyrida.Application.Common.Mapping;

/// <summary>
/// Configuration for DTOs mappings
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
public class MappingConfig : IRegister
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Custom configuration for Mapster
    /// </summary>
    /// <param name="config">The configuration to be configured</param>
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<Directory, DirectoryEntity>.NewConfig()
            .Map(dest => dest.Path, src => src.Id.Path)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.DateCreated, src => src.DateCreated)
            .Map(dest => dest.DateModified, src => src.DateModified)
            .Map(dest => dest.Items, src => src.Items.Adapt<List<FileSystemItemEntity>>());

        TypeAdapterConfig<File, FileEntity>.NewConfig()
            .Map(dest => dest.Path, src => src.Id.Path)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.DateModified, src => src.DateModified)
            .Map(dest => dest.DateCreated, src => src.DateCreated)
            .Map(dest => dest.Size, src => src.Size);
    }
    #endregion
}