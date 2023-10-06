#region ========================================================================= USING =====================================================================================
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Infrastructure.Localization;

/// <summary>
/// Custom collection item for translation service 
/// </summary>
/// <remarks>
/// Creation Date: 18th of January, 2022
/// </remarks>
public class TranslationItem
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private string? lastValue;
    private string? lastLanguage;
    #endregion

    #region ==================================================================== PROPERTIES =================================================================================
    public string Ro { get; set; }
    public string En { get; set; }
    public string De { get; set; }
    public Terms Translation { get; set; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="translation">The translation that acts like key for this object</param>
    /// <param name="ro">The Romanian translation to store</param>
    /// <param name="en">The English translation to store</param>
    /// <param name="de">The German translation to store</param>
    public TranslationItem(Terms translation, string ro, string en, string de)
    {
        Translation = translation;
        Ro = ro;
        En = en;
        De = de;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Operator overloading to support easy adding to the <see cref="TranslationsCollection"/>
    /// </summary>
    /// <param name="param">Paramaters required in <see cref="TranslationItem"/> as <see cref="System.ValueTuple"/></param>
    public static implicit operator TranslationItem((Terms term, string ro, string en, string de) param)
    {            
        return new TranslationItem(param.term, param.ro, param.en, param.de);
    }

    /// <summary>
    /// Custom indexer to get the translation corresponding to the language short sympol
    /// </summary>
    /// <param name="language">Language short sympol</param>
    /// <returns>The translation corresponding</returns>
    public string this[string language]
    {
        get
        {
            if (language == lastLanguage)
                return lastValue ?? Ro;
            else
            {

                lastLanguage = language;
                lastValue = language == "ro" ? Ro : language == "en" ? En : De;
                return lastValue;
            }
        }
    }

    /// <summary>
    /// Custom indexer to get the translation corresponding to the language short sympol
    /// </summary>
    /// <param name="language">Language short sympol</param>
    /// <returns>The translation corresponding</returns>
    public string this[Language language]
    {
        get
        {
            if (language.ToString().ToLower()[..2] == lastLanguage)
                return lastValue ?? Ro;
            else
            {

                lastLanguage = language.ToString().ToLower()[..2];
                lastValue = lastLanguage == "ro" ? Ro : lastLanguage == "en" ? En : De;
                return lastValue;
            }
        }
    }
    #endregion
}