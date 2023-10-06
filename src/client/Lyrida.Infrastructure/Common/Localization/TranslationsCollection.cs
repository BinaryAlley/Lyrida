#region ========================================================================= USING =====================================================================================
using System.Linq;
using System.Collections.Generic;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Infrastructure.Localization;

/// <summary>
/// Custom collection for translation service
/// </summary>
/// <remarks>
/// Creation Date: 18th of January, 2022
/// </remarks>
public class TranslationsCollection : List<TranslationItem>
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Custom indexing this collection by <see cref="Terms"/>
    /// </summary>
    /// <param name="term">The index to use for the custom indexing</param>
    /// <returns>The <see cref="TranslationItem"/> corresponding to <paramref name="term"/></returns>
    public TranslationItem this[Terms term]
    {
        get => this.First(x => x.Translation == term);
    }
    #endregion
}