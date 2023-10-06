#region ========================================================================= USING =====================================================================================
using System;
#endregion

namespace Lyrida.DataAccess.Common.Attributes;

/// <summary>
/// Custom attribute for DTO properties, mapping their names to the names used in the storage medium
/// </summary>
/// <remarks>
/// Creation Date: 17th of July, 2023
/// <para>
/// Cannot be internal, see https://github.com/autofac/Autofac/issues/497 and https://github.com/dotnet/runtime/issues/6625
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Property)]
public class MapsToAttribute : Attribute
{
    #region ================================================================== PROPERTIES ===================================================================================
    public string? Name { get; set; }
    #endregion
}