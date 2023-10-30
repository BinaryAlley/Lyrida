#region ========================================================================= USING =====================================================================================
using Mapster;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Application.Common.DTO.FileSystem;
using Lyrida.Api.Common.DTO.Authentication;
using Lyrida.Application.Common.DTO.Authentication;
#endregion

namespace Lyrida.Api.Common.Mapping;

/// <summary>
/// Configuration for DTOs mappings
/// </summary>
/// <remarks>
/// Creation Date: 23rd of October, 2023
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
        TypeAdapterConfig<AuthenticationResultDto, LoginResponseDto>.NewConfig()
            .Map(dest => dest.Id, src => src.User.Id)
            .Map(dest => dest.FirstName, src => src.User.FirstName)
            .Map(dest => dest.LastName, src => src.User.LastName)
            .Map(dest => dest.Email, src => src.User.Email)
            .Map(dest => dest.UsesTotp, src => src.User.UsesTotp)
            .Map(dest => dest.Token, src => src.Token);
        TypeAdapterConfig<AuthenticationResultDto, AuthenticationResponseDto>.NewConfig()
            .Map(dest => dest.Id, src => src.User.Id)
            .Map(dest => dest.FirstName, src => src.User.FirstName)
            .Map(dest => dest.LastName, src => src.User.LastName)
            .Map(dest => dest.Email, src => src.User.Email)
            .Map(dest => dest.Token, src => src.Token);
        TypeAdapterConfig<RegistrationResultDto, AuthenticationResponseDto>.NewConfig()
            .Map(dest => dest.Id, src => src.User.Id)
            .Map(dest => dest.FirstName, src => src.User.FirstName)
            .Map(dest => dest.LastName, src => src.User.LastName)
            .Map(dest => dest.Email, src => src.User.Email)
            .Map(dest => dest.TotpSecret, src => src.User.TotpSecret);
    }
    #endregion
}