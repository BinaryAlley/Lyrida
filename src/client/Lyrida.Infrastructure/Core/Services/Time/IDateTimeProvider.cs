#region ========================================================================= USING =====================================================================================
using System;
#endregion

namespace Lyrida.Infrastructure.Core.Services.Time;

/// <summary>
/// Interface for time related concerns
/// </summary>
/// <remarks>
/// Creation Date: 08th of July, 2023
/// </remarks>
public interface IDateTimeProvider
{
    #region ==================================================================== PROPERTIES =================================================================================
    DateTime UtcNow { get; }
    #endregion
}