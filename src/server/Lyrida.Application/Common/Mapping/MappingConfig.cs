#region ========================================================================= USING =====================================================================================
using Mapster;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Application.Common.DTO.FileSystem;
using System;
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
        TypeAdapterConfig<Directory, DirectoryDto>.NewConfig()
            .Map(dest => dest.Path, src => src.Id.Path)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.DateCreated, src => src.DateCreated)
            .Map(dest => dest.DateModified, src => src.DateModified)
            .Map(dest => dest.Items, src => src.Items.Adapt<List<FileSystemItemDto>>());

        TypeAdapterConfig<File, FileDto>.NewConfig()
            .Map(dest => dest.Path, src => src.Id.Path)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.DateModified, src => src.DateModified)
            .Map(dest => dest.DateCreated, src => src.DateCreated)
            .Map(dest => dest.Size, src => src.Size);

        TypeAdapterConfig<DataAccess.Common.DTO.Pages.PageDto, DTO.Pages.PageDto>
            .NewConfig()
            .Map(dest => dest.PageId, src => ConvertUuidString(src.PageIdString))
            .Map(dest => dest.EnvironmentId, src => ConvertUuidString(src.EnvironmentIdString));

        TypeAdapterConfig<DataAccess.Common.DTO.Environments.FileSystemDataSourceDto, DTO.Environments.FileSystemDataSourceDto>
            .NewConfig()
            .Map(dest => dest.EnvironmentId, src => ConvertUuidString(src.EnvironmentIdString));
    }

    /// <summary>
    /// Converts a string representation of a unique identifier (UUID) to its Guid equivalent.
    /// </summary>
    /// <param name="uuidString">The string representation of the UUID to convert.</param>
    /// <returns>
    /// A Guid equivalent to the UUID contained in <paramref name="uuidString"/>, or <see cref="Guid.Empty"/> if the string is null, empty, or not of a valid format.
    /// </returns>
    public static Guid ConvertUuidString(string? uuidString)
    {
        if (Guid.TryParse(uuidString, out Guid temp))
            return temp;
        return Guid.Empty;
    }
    #endregion
}