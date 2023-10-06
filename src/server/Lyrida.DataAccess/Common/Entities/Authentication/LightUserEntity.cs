#region ========================================================================= USING =====================================================================================
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Attributes;
#endregion

namespace Lyrida.DataAccess.Common.Entities.Authentication;

/// <summary>
/// Trimmed down deserialization model for the Users storage container, used in search lists that only need the users's name and id
/// </summary>
/// <remarks>
/// Creation Date: 22nd of February, 2022
/// </remarks>
public sealed class LightUserEntity : IStorageEntity
{
    #region ==================================================================== PROPERTIES =================================================================================
    [IgnoreOnCommand]
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
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