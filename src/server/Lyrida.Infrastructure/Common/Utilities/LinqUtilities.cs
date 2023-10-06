#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using System.Collections.Generic;
#endregion

namespace Lyrida.Infrastructure.Common.Utilities;

/// <summary>
/// LINQ extension methods
/// </summary>
/// <remarks>
/// Creation Date: 14th of February, 2022
/// </remarks>
public static class LinqUtilities
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Determines whether a sequence contains no elements.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/></typeparam>
    /// <param name="source">The collection to check</param>
    /// <returns>True, if the sequence contains no elements, False otherwise</returns>
    public static bool None<TSource>(this IEnumerable<TSource>? source)
    {
        return source?.Any() != true;
    }

    /// <summary>
    /// Determines whether no element of a sequence satisfies a condition.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/></typeparam>
    /// <param name="source">A collection on which to apply <paramref name="predicate"/></param>
    /// <param name="predicate">A function to test each element of <paramref name="source"/> for a condition</param>
    /// <returns>True if no elements of <paramref name="source"/> pass the test in <paramref name="predicate"/>, False otherwise</returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="predicate"/> is null</exception>
    public static bool None<TSource>(this IEnumerable<TSource>? source, Func<TSource, bool> predicate)
    {
        if (predicate == null)
            throw new ArgumentException("predicate cannot be null!");
        return source?.Any(predicate) != true;
    }
    #endregion
}