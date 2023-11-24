#region ========================================================================= USING =====================================================================================
using System;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Attributes;
#endregion

namespace Lyrida.DataAccess.Common.DTO.Authentication;

/// <summary>
/// User data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 09th of July, 2023
/// </remarks>
public sealed class UserDto : IStorageDto
{
    #region ==================================================================== PROPERTIES =================================================================================
    [IgnoreOnCommand]
    [MapsTo(Name = "id")]
    public int Id { get; set; }
    [MapsTo(Name = "username")]
    public string Username { get; set; } = null!;
    [MapsTo(Name = "password")]
    public string Password { get; set; } = null!;
    [MapsTo(Name = "totp_secret")]
    public string? TotpSecret { get; set; } = null!;
    [MapsTo(Name = "verification_token")]
    public string? VerificationToken { get; set; } = null!;
    [MapsTo(Name = "verification_token_created")]
    public DateTime? VerificationTokenCreated { get; set; }
    [IgnoreOnCommand]
    [MapsTo(Name = "created")]
    public DateTime Created { get; set; }
    [IgnoreOnCommand]
    [MapsTo(Name = "updated")]
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
    #endregion
}