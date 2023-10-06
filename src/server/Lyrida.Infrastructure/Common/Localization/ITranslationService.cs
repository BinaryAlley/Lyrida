#region ========================================================================= USING =====================================================================================
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Infrastructure.Localization;

/// <summary>
/// Interface for strings translation
/// </summary>
/// <remarks>
/// Creation Date: 18th of January, 2022
/// </remarks>
public interface ITranslationService
{
    #region ==================================================================== PROPERTIES =================================================================================
    public Language Language { get; set; }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Translates a string identified by <paramref name="messageId"/>
    /// </summary>
    /// <param name="messageId">The index of the string to be translated</param>
    /// <returns>The translation of the item identified by <paramref name="messageId"/></returns>
    string Translate(Terms messageId);
    #endregion
}