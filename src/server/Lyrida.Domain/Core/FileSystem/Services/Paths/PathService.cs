#region ========================================================================= USING =====================================================================================
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
using Lyrida.Domain.Core.FileSystem.Services.Platform;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Paths;

/// <summary>
/// Service defining methods for handling path-related operations
/// </summary>
/// <remarks>
/// Creation Date: 02nd of October, 2023
/// </remarks>
public class PathService : IPathService
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IPlatformContext platformContext;
    #endregion

    #region ==================================================================== PROPERTIES =================================================================================
    public char PathSeparator 
    {
        get { return platformContext.PathStrategy.PathSeparator; }
    }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="platformContextManager">Injected facade service for platform contextual services</param>
    public PathService(IPlatformContextManager platformContextManager)
    {
        platformContext = platformContextManager.GetCurrentContext();    
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Checks if <paramref name="path"/> is a valid path.
    /// </summary>
    /// <param name="path">The path to be checked.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> is a valid path, <see langword="false"/> otherwise.</returns>
    public bool IsValidPath(string path)
    {
        ErrorOr<FileSystemPathId> newPathResult = FileSystemPathId.Create(path);
        if (newPathResult.IsError)
            return false;
        return platformContext.PathStrategy.IsValidPath(newPathResult.Value);
    }

    /// <summary>
    /// Tries to combine <paramref name="path"/> with <paramref name="name"/>.
    /// </summary>
    /// <param name="path">The path to be combined.</param>
    /// <param name="path">The name to be combined with the path.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the combined path, or an error.</returns>
    public ErrorOr<string> CombinePath(string path, string name)
    {
        ErrorOr<FileSystemPathId> newPathResult = FileSystemPathId.Create(path);
        if (newPathResult.IsError)
            return newPathResult.Errors;
        ErrorOr<FileSystemPathId> combinedPathResult = platformContext.PathStrategy.CombinePath(newPathResult.Value, name);
        if (combinedPathResult.IsError)
            return combinedPathResult.Errors;
        return combinedPathResult.Value.Path;
    }

    /// <summary>
    /// Parses <paramref name="path"/> into path segments.
    /// </summary>
    /// <param name="path">The path to be parsed.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the path segments, or an error.</returns>
    public ErrorOr<IEnumerable<PathSegment>> ParsePath(string path)
    {
        ErrorOr<FileSystemPathId> newPathResult = FileSystemPathId.Create(path);
        if (newPathResult.IsError)
            return newPathResult.Errors;
        return platformContext.PathStrategy.ParsePath(newPathResult.Value);
    }

    /// <summary>
    /// Goes up one level from <paramref name="path"/>, and returns the path segments.
    /// </summary>
    /// <param name="path">The path from which to navigate up one level.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the path segments of the path up one level from <paramref name="path"/>, or an error.</returns>
    public ErrorOr<IEnumerable<PathSegment>> GoUpOneLevel(string path)
    {
        ErrorOr<FileSystemPathId> newPathResult = FileSystemPathId.Create(path);
        if (newPathResult.IsError)
            return newPathResult.Errors;
        return platformContext.PathStrategy.GoUpOneLevel(newPathResult.Value);
    }

    /// <summary>
    /// Returns a collection of characters that are invalid for paths.
    /// </summary>
    /// <returns>A collection of characters that are invalid in the context of paths</returns>
    public char[] GetInvalidPathCharsForPlatform()
    {
        return platformContext.PathStrategy.GetInvalidPathCharsForPlatform();
    }
    #endregion
}