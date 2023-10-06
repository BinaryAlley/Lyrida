#region ========================================================================= USING =====================================================================================
using System;
#endregion

namespace Lyrida.DataAccess.Common.Attributes;

/// <summary>
/// Custom attribute for DTO properties that should be ignored in the process of selects
/// </summary>
/// <remarks>
/// Creation Date: 16th of August, 2023
/// </remarks>
[AttributeUsage(AttributeTargets.Property)]
internal class IgnoreOnQueryAttribute : Attribute
{

}