#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
#endregion

namespace Lyrida.Infrastructure.Common.Utilities;

/// <summary>
/// Exception extension methods
/// </summary>
/// <remarks>
/// Creation Date: 18th of April, 2022
/// <para>
/// based on https://stackoverflow.com/a/62283895
/// </para>
/// </remarks>
public static class ExceptionUtilities
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Concatenates all inner exception(s) messages of <paramref name="ex"/>
    /// </summary>
    /// <param name="ex">The exception for which to get the inner exception(s) message</param>
    /// <returns>A string representing the concatenated messages of all inner expcetion(s) of <paramref name="ex"/></returns>
    public static string AggregateMessages(this Exception ex)
    {
        return ex.GetInnerExceptions().Aggregate(new StringBuilder(), (stringBuilder, exception) => stringBuilder.AppendLine(exception.Message + " -> "), stringBuilder => stringBuilder.ToString());
    }

    /// <summary>
    /// Recursively gets the inner exception(s) of <paramref name="exception"/>, at <paramref name="maximumDepth"/> depth
    /// </summary>
    /// <param name="exception">The exception for which to get the inner exception(s)</param>
    /// <param name="maximumDepth">The depth of the recursivity of getting the inner exception(s) of <paramref name="exception"/></param>
    /// <returns>A list of exceptions representing the list of inner exception(s) of <paramref name="exception"/></returns>
    public static IEnumerable<Exception> GetInnerExceptions(this Exception? exception, int maximumDepth = 5)
    {
        // check if there are any more inner exceptions to return
        if (exception == null || maximumDepth <= 0)
            yield break;
        // yield the current level exception itself
        yield return exception;
        // if the exception is an AggregateException, treat it differently and get its all exceptions
        if (exception is AggregateException aggregateException)
            foreach (var i in aggregateException.InnerExceptions.SelectMany(innerException => GetInnerExceptions(innerException, maximumDepth - 1)))
                yield return i;
        // if it's a normal exception, recursively get its list of inner exceptions and yield them
        foreach (Exception? innerException in GetInnerExceptions(exception.InnerException, maximumDepth - 1))
            yield return innerException;
    }
    #endregion
}