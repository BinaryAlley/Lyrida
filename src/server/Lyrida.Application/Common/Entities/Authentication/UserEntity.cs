#region ========================================================================= USING =====================================================================================
using System;
using Mapster;
#endregion

namespace Lyrida.Application.Common.Entities.Authentication;

/// <summary>
/// User data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 09th of July, 2023
/// </remarks>
public sealed class UserEntity 
{
    #region ==================================================================== PROPERTIES =================================================================================
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? VerificationToken { get; set; }
    public DateTime? VerificationTokenCreated { get; set; }
    public bool IsVerified { get; set; }
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
        return Id + " :: " + Email;
    }

    /// <summary>
    /// Maps between this entity and the coresponding persistance entity
    /// </summary>
    /// <returns>A data storage entity representation of this entity</returns>
    public DataAccess.Common.Entities.Authentication.UserEntity ToStorageEntity()
    {
        return this.Adapt<DataAccess.Common.Entities.Authentication.UserEntity>();
    }
    #endregion
}
