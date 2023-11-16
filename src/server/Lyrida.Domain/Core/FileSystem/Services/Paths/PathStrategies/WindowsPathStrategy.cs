﻿#region ========================================================================= USING =====================================================================================
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
    #region ==================================================================== PROPERTIES =================================================================================
    public char PathSeparator
    {
        get { return '\\'; }
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Checks if <paramref name="path"/> is a valid path.
    /// </summary>
    /// <param name="path">The path to be checked.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> is a valid path, <see langword="false"/> otherwise.</returns>
    public bool IsValidPath(FileSystemPathId path)
    {
        // check for invalid path characters
        char[] invalidChars = GetInvalidPathCharsForPlatform();
        if (path.Path.IndexOfAny(invalidChars) >= 0)
            return false;
        // regular expression to match valid absolute paths
        // this allows drive letters (e.g., C:\) and UNC paths (e.g., \\server\share)
        var pathPattern = @"^[a-zA-Z]:\\(?:[a-zA-Z0-9\s().-]+\\)*[a-zA-Z0-9\s().-]*\\?$";
        return Regex.IsMatch(path.Path, pathPattern);
    }

    /// <summary>
    /// Tries to combine <paramref name="path"/> with <paramref name="name"/>.
    /// </summary>
    /// <param name="path">The path to be combined.</param>
    /// <param name="name">The name to be combined with the path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the combined path, or an error.</returns>
    public ErrorOr<FileSystemPathId> CombinePath(FileSystemPathId path, string name)
    {
        if (name == null)
            return Errors.FileSystem.NameCannotBeEmptyError;
        // trim any directory separator characters from the end of the path
        string subpath = path.Path.TrimEnd(PathSeparator);
        // if the name begins with a directory separator, remove it
        name = name.TrimStart(PathSeparator);
        // combine the two parts with the Windows directory separator character
        return FileSystemPathId.Create(subpath + PathSeparator + name);
    }

    /// <summary>
    /// Parses <paramref name="path"/> into path segments.
    /// </summary>
    /// <param name="path">The path to be parsed.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the path segments, or an error.</returns>
    public ErrorOr<IEnumerable<PathSegment>> ParsePath(FileSystemPathId path)
    {
        // Windows paths usually start with a drive letter and colon, e.g., "C:"
        if (!path.Path.Contains(':') || !path.Path.Contains(PathSeparator) || !char.IsLetter(path.Path[0]) || path.Path[1] != ':' || path.Path[2] != PathSeparator)
            return Errors.FileSystem.InvalidPathError;
        return ErrorOrFactory.From(GetPathSegments());
        IEnumerable<PathSegment> GetPathSegments()
        {
            // the drive segment
            yield return new PathSegment(path.Path[..2], isDirectory: false, isDrive: true);
            // extract the other segments
            var segments = path.Path[3..].Split(new[] { PathSeparator }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < segments.Length; i++)
            {
                var segment = segments[i];
                bool isDirectory;
                if (segment.Contains('.'))                   
                    isDirectory = (i != segments.Length - 1) || path.Path.EndsWith(PathSeparator); // check if it's the last segment or if the next segment also contains a path delimiter
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
    public ErrorOr<IEnumerable<PathSegment>> GoUpOneLevel(FileSystemPathId path)
    {
        // validation: ensure the path is not null or empty
        if (!IsValidPath(path))
            return Errors.FileSystem.InvalidPathError;
        // if path is just a drive letter followed by ":\", return null
        if (Regex.IsMatch(path.Path, @"^[a-zA-Z]:\\?$"))
            return Errors.FileSystem.CannotNavigateUpError;
        // trim trailing slash for consistent processing
        string tempPath = path.Path;
        if (tempPath.EndsWith("\\"))
            tempPath = tempPath.TrimEnd(PathSeparator);
        // find the last occurrence of a slash
        int lastIndex = tempPath.LastIndexOf(PathSeparator);
        // if there's no slash found (shouldn't happen due to previous steps), or if we are at the root level after trimming, return error
        if (lastIndex < 0)
            return Errors.FileSystem.CannotNavigateUpError;
        // if we are at the drive root level after trimming, return drive root, otherwise, return the path up to the last slash
        ErrorOr<FileSystemPathId> newPathResult = FileSystemPathId.Create(lastIndex == 2 && tempPath[1] == ':' ? tempPath[..3] : tempPath[..lastIndex]);
        if (newPathResult.IsError)
            return newPathResult.Errors;
        return ParsePath(newPathResult.Value);
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