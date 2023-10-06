#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Domain.Common.Errors;
using System.Text.RegularExpressions;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Paths.PathStrategies;

/// <summary>
/// Service defining methods for handling path-related operations on Windows platform
/// </summary>
/// <remarks>
/// Creation Date: 01st of October, 2023
/// </remarks>
public class WindowsPathStrategy : IWindowsPathStrategy
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Checks if <paramref name="path"/> is a valid path.
    /// </summary>
    /// <param name="path">The path to be checked.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> is a valid path, <see langword="false"/> otherwise.</returns>
    public bool IsValidPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;
        // check for invalid path characters
        char[] invalidChars = GetInvalidPathCharsForPlatform();
        if (path.IndexOfAny(invalidChars) >= 0)
            return false;
        // regular expression to match valid absolute paths
        // this allows drive letters (e.g., C:\) and UNC paths (e.g., \\server\share)
        var pathPattern = @"^(?:[a-zA-Z]:\\|\\\\[\w.]+\\[\w.$]+)\\?(?:[\w]+\\?)*$";
        return Regex.IsMatch(path, pathPattern);
    }

    /// <summary>
    /// Parses <paramref name="path"/> into path segments.
    /// </summary>
    /// <param name="path">The path to be parsed.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the path segments, or an error.</returns>
    public ErrorOr<IEnumerable<PathSegment>> ParsePath(string path)
    {
        // Validation: Ensure the path is not null or empty.
        if (string.IsNullOrEmpty(path))
            return Errors.FileSystem.InvalidPath;
        // Windows paths usually start with a drive letter and colon, e.g., "C:"
        if (!char.IsLetter(path[0]) || path[1] != ':' || path[2] != '\\')
            return Errors.FileSystem.InvalidPath;
        return ErrorOrFactory.From(GetPathSegments());
        IEnumerable<PathSegment> GetPathSegments()
        {
            // the drive segment
            yield return new PathSegment(path[..2], isDirectory: false, isDrive: true);
            // extract the other segments
            foreach (var segment in path[3..].Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries))
            {                
                bool isDirectory = !segment.Contains('.'); // Assuming segments with extensions are files
                yield return new PathSegment(segment, isDirectory, isDrive: false);
            }
        }
    }

    /// <summary>
    /// Goes up one level from <paramref name="path"/>, and returns the path segments.
    /// </summary>
    /// <param name="path">The path from which to navigate up one level.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the path segments of the path up one level from <paramref name="path"/>, or an error.</returns>
    public ErrorOr<IEnumerable<PathSegment>> GoUpOneLevel(string path)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns a collection of characters that are invalid for paths.
    /// </summary>
    /// <returns>A collection of characters that are invalid in the context of paths</returns>
    public char[] GetInvalidPathCharsForPlatform()
    {
        return new char[] { '<', '>', '"', '/', '|', '?', '*' };
    }
    #endregion
}