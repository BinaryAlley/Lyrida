#region ========================================================================= USING =====================================================================================
using System;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.Attributes;
#endregion

namespace Lyrida.DataAccess.Common.Entities.Common;

/// <summary>
/// Deserialization model for SQL time related functions
/// </summary>
/// <remarks>
/// Creation Date: 09th of February, 2022
/// </remarks>
public sealed class TimeEntity : IStorageEntity
{
    #region ================================================================== PROPERTIES ===================================================================================
    [IgnoreOnCommand]
    public int Id { get; set; }
    public DateTime CurrentDate { get; set; }
    #endregion
}