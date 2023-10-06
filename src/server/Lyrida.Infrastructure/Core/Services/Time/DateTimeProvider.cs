#region ========================================================================= USING =====================================================================================
using System;
#endregion

namespace Lyrida.Infrastructure.Core.Services.Time;

/// <summary>
/// Service for time related concerns
/// </summary>
/// <remarks>
/// Creation Date: 08th of July, 2023
/// </remarks>
public class DateTimeProvider : IDateTimeProvider
{
    #region ==================================================================== PROPERTIES =================================================================================
    public DateTime UtcNow
    {
        get { return DateTime.UtcNow; }
    }
    #endregion
}