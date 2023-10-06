#region ========================================================================= USING =====================================================================================
using System;
#endregion

namespace Lyrida.Infrastructure.Common.Time;

/// <summary>
/// Interface for time related functionality
/// </summary>
/// <remarks>
/// Creation Date: 24th of August, 2023
/// </remarks>
public interface ITimeService
{
    #region ==================================================================== PROPERTIES =================================================================================
    DateTime Now { get; }
    DateTime UtcNow { get; }
    #endregion
}
