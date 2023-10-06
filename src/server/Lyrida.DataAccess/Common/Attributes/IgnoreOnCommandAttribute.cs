#region ========================================================================= USING =====================================================================================
using System;
#endregion

namespace Lyrida.DataAccess.Common.Attributes;

/// <summary>
/// Custom attribute for DTO properties that should be ignored in the process of inserts-updates-deletes
/// </summary>
/// <remarks>
/// Creation Date: 17th of July, 2023
/// </remarks>
[AttributeUsage(AttributeTargets.Property)]
internal class IgnoreOnCommandAttribute : Attribute
{

}