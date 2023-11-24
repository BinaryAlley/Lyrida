#region ========================================================================= USING =====================================================================================
using System;
using Mapster;
#endregion

namespace Lyrida.Application.Common.DTO.Authentication;

/// <summary>
/// User data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 09th of July, 2023
/// </remarks>
public sealed class UserDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? TotpSecret { get; set; } = null!;
    public bool UsesTotp { get; set; }
    public string? VerificationToken { get; set; }
    public DateTime? VerificationTokenCreated { get; set; }
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
        return Id + " :: " + Username;
    }

    /// <summary>
    /// Maps between this DTO and the coresponding persistance DTO
    /// </summary>
    /// <returns>A data storage DTO representation of this DTO</returns>
    public DataAccess.Common.DTO.Authentication.UserDto ToStorageDto()
    {
        return this.Adapt<DataAccess.Common.DTO.Authentication.UserDto>();
    }
    #endregion
}