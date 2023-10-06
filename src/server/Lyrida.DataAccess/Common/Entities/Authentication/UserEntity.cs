#region ========================================================================= USING =====================================================================================
using System;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Attributes;
#endregion

namespace Lyrida.DataAccess.Common.Entities.Authentication;

/// <summary>
/// User data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 09th of July, 2023
/// </remarks>
public sealed class UserEntity : IStorageEntity
{
    #region ==================================================================== PROPERTIES =================================================================================
    [IgnoreOnCommand]
    [MapsTo(Name = "id")]
    public int Id { get; set; }
    [MapsTo(Name = "first_name")]
    public string FirstName { get; set; } = null!;
    [MapsTo(Name = "last_name")]
    public string LastName { get; set; } = null!;
    [MapsTo(Name = "email")]
    public string Email { get; set; } = null!;
    [MapsTo(Name = "password")]
    public string Password { get; set; } = null!;
    [MapsTo(Name = "verification_token")]
    public string? VerificationToken { get; set; } = null!;
    [MapsTo(Name = "verification_token_created")]
    public DateTime? VerificationTokenCreated { get; set; }
    [MapsTo(Name = "is_verified")]
    public bool IsVerified { get; set; }
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
        return Id + " :: " + Email;
    }
    #endregion
}