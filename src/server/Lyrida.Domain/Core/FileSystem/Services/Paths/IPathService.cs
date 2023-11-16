#region ========================================================================= USING =====================================================================================
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Paths;

/// <summary>
/// Interface for the service for handling file system paths
/// </summary>
/// <remarks>
/// Creation Date: 02nd of October, 2023
/// </remarks>
public interface IPathService
{
    #region ==================================================================== PROPERTIES =================================================================================
    char PathSeparator { get; }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Checks if <paramref name="path"/> is a valid path.
    /// </summary>
    /// <param name="path">The path to be checked.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> is a valid path, <see langword="false"/> otherwise.</returns>
    bool IsValidPath(string path);

    /// <summary>
    /// Tries to combine <paramref name="path"/> with <paramref name="name"/>.
    /// </summary>
    /// <param name="path">The path to be combined.</param>
    /// <param name="path">The name to be combined with the path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the combined path, or an error.</returns>
    ErrorOr<string> CombinePath(string path, string name);

    /// <summary>
    /// Parses <paramref name="path"/> into path segments.
    /// </summary>
    /// <param name="path">The path to be parsed.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the path segments, or an error.</returns>
    ErrorOr<IEnumerable<PathSegment>> ParsePath(string path);

    /// <summary>
    /// Goes up one level from <paramref name="path"/>, and returns the path segments.
    /// </summary>
    /// <param name="path">The path from which to navigate up one level.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the path segments of the path up one level from <paramref name="path"/>, or an error.</returns>
    ErrorOr<IEnumerable<PathSegment>> GoUpOneLevel(string path);

    /// <summary>
    /// Returns a collection of characters that are invalid for paths.
    /// </summary>
    /// <returns>A collection of characters that are invalid in the context of paths.</returns>
    char[] GetInvalidPathCharsForPlatform();
    #endregion
}