#region ========================================================================= USING =====================================================================================
using System.Collections.Generic;
using Lyrida.Domain.Common.Models;
#endregion

namespace Lyrida.Domain.Core.FileSystem.ValueObjects;

/// <summary>
/// Value object uniquely identifying path segments
/// </summary>
/// <remarks>
/// Creation Date: 01st of October, 2023
/// </remarks>
public class PathSegment : ValueObject
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string Name { get; }
    public bool IsDirectory { get; }
    public bool IsDrive { get; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="name"></param>
    /// <param name="isDirectory"></param>
    /// <param name="isDrive"></param>
    public PathSegment(string name, bool isDirectory, bool isDrive)
    {
        Name = name;
        IsDirectory = isDirectory;
        IsDrive = isDrive;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <inheritdoc/>
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return IsDirectory;
        yield return IsDrive;
    }
    #endregion
}