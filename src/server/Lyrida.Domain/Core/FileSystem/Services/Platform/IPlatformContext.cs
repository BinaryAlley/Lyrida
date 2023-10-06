﻿#region ========================================================================= USING =====================================================================================
using Lyrida.Domain.Core.FileSystem.Services.Paths.PathStrategies;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Platform;

/// <summary>
/// Interface for defining the contract of a platform-specific context.
/// </summary>
/// <remarks>
/// Creation Date: 04th of September, 2023
/// </remarks>
public interface IPlatformContext
{
    #region ==================================================================== PROPERTIES =================================================================================
    IPathStrategy PathService { get; }
    #endregion
}