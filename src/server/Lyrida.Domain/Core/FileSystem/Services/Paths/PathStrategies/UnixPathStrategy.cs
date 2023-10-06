﻿#region ========================================================================= USING =====================================================================================
using System;
using ErrorOr;
using System.Linq;
using System.Collections.Generic;
using Lyrida.Domain.Common.Errors;
using System.Text.RegularExpressions;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Paths.PathStrategies;

/// <summary>
/// Service defining methods for handling path-related operations on UNIX platform
/// </summary>
/// <remarks>
/// Creation Date: 01st of October, 2023
/// </remarks>
public class UnixPathStrategy : IUnixPathStrategy
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
        // check for relative paths
        if (path.StartsWith("./") || path.StartsWith("../"))
            return false;
        var pathPattern = @"^\/[\w\-\.]+(\/[\w\-\.]+)*\/?$";
        return Regex.IsMatch(path, pathPattern);
    }

    /// <summary>
    /// Parses <paramref name="path"/> into path segments.
    /// </summary>
    /// <param name="path">The path to be parsed.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the path segments, or an error.</returns>
    public ErrorOr<IEnumerable<PathSegment>> ParsePath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return Errors.FileSystem.InvalidPath;
        // if path starts with anything other than '/', it's considered relative and invalid for this parser
        if (!path.StartsWith('/'))
            return Errors.FileSystem.InvalidPath;
        // get the path segments
        IEnumerable<PathSegment> segments = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(segment =>
            {                              
                bool isDirectory = !segment.Contains('.'); // assuming segments with extensions are files
                return new PathSegment(segment, isDirectory, isDrive: false);
            });
        return ErrorOrFactory.From(segments);
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
        return new char[] { '\0' };
    }
    #endregion
}