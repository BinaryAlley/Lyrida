#region ========================================================================= USING =====================================================================================
using ErrorOr;
using System.Diagnostics;
using System.Collections.Generic;
using Lyrida.Domain.Common.Models;
using Lyrida.Domain.Common.Errors;
#endregion

namespace Lyrida.Domain.Core.FileSystem.ValueObjects;

/// <summary>
/// Value object uniquely identifying file system items
/// </summary>
/// <remarks>
/// Creation Date: 22nd of September, 2023
/// </remarks>
[DebuggerDisplay("{Path}")]
public sealed class FileSystemPathId : ValueObject
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string Path { get; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="path">The path of the file system item.</param>
    private FileSystemPathId(string path)
    {
        Path = path;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <inheritdoc />
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Path;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="FileSystemPathId"/> class.
    /// </summary>
    /// <param name="path">The path of the file system item.</param>
    /// <returns>
    /// An <see cref="ErrorOr{T}"/> containing either a successfully created <see cref="FileSystemPathId"/> or an error message.
    /// </returns>
    public static ErrorOr<FileSystemPathId> Create(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Errors.FileSystem.InvalidPath;
        return new FileSystemPathId(path);
    }
    #endregion
}