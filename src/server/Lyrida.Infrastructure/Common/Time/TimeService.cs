#region ========================================================================= USING =====================================================================================
using System;
#endregion

namespace Lyrida.Infrastructure.Common.Time;

/// <summary>
/// Handles time related functionality
/// </summary>
/// <remarks>
/// Creation Date: 24th of August, 2023
/// </remarks>
public class TimeService : ITimeService
{
    #region ==================================================================== PROPERTIES =================================================================================
    public DateTime Now { get { return DateTime.Now; } }
    public DateTime UtcNow { get { return DateTime.UtcNow; } }
    #endregion
}