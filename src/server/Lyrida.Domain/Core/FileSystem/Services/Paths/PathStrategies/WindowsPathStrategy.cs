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
        var pathPattern = @"^[a-zA-Z]:\\(?:[a-zA-Z0-9\s().-]+\\)*[a-zA-Z0-9\s().-]*\\?$";
        return Regex.IsMatch(path, pathPattern);
    }

    /// <summary>
    /// Parses <paramref name="path"/> into path segments.
    /// </summary>
    /// <param name="path">The path to be parsed.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the path segments, or an error.</returns>
    public ErrorOr<IEnumerable<PathSegment>> ParsePath(string path)
    {
        // validation: ensure the path is not null or empty.
        if (string.IsNullOrWhiteSpace(path))
            return Errors.FileSystem.InvalidPath;
        // Windows paths usually start with a drive letter and colon, e.g., "C:"
        if (!path.Contains(':') || !path.Contains('\\') || !char.IsLetter(path[0]) || path[1] != ':' || path[2] != '\\')
            return Errors.FileSystem.InvalidPath;
        return ErrorOrFactory.From(GetPathSegments());
        IEnumerable<PathSegment> GetPathSegments()
        {
            // the drive segment
            yield return new PathSegment(path[..2], isDirectory: false, isDrive: true);
            // extract the other segments
            var segments = path[3..].Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < segments.Length; i++)
            {
                var segment = segments[i];
                bool isDirectory;
                if (segment.Contains('.'))                   
                    isDirectory = (i != segments.Length - 1) || path.EndsWith('\\'); // check if it's the last segment or if the next segment also contains a path delimiter
                else
                    isDirectory = true;
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
        // validation: ensure the path is not null or empty
        if (!IsValidPath(path))
            return Errors.FileSystem.InvalidPath;
        // if path is just a drive letter followed by ":\", return null
        if (Regex.IsMatch(path, @"^[a-zA-Z]:\\?$"))
            return Errors.FileSystem.CannotNavigateUp;
        // trim trailing slash for consistent processing
        if (path.EndsWith("\\"))
            path = path.TrimEnd('\\');
        // find the last occurrence of a slash
        int lastIndex = path.LastIndexOf('\\');
        // if there's no slash found (shouldn't happen due to previous steps), or if we are at the root level after trimming, return error
        if (lastIndex < 0)
            return Errors.FileSystem.CannotNavigateUp;
        // if we are at the drive root level after trimming, return drive root
        if (lastIndex == 2 && path[1] == ':')
            return ParsePath(path[..3]);
        // return the path up to the last slash
        return ParsePath(path[..lastIndex]);
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