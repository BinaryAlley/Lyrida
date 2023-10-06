#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;
using Autofac.Core.Activators.Reflection;
#endregion

namespace Lyrida.Infrastructure.Common.DependencyInjection;

/// <summary>
/// Custom implementation of IConstructorFinder that overrides Autofac's default behavior of looking up just private constructors
/// </summary>
/// <remarks>
/// Creation Date: 14th of October, 2018
/// <para>
/// https://stackoverflow.com/questions/52793062/how-to-resolve-public-class-with-internal-constructor-on-autofac/52799160#52799160
/// </para>
/// </remarks>
public class InternalConstructorFinder : IConstructorFinder
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private static readonly ConcurrentDictionary<Type, ConstructorInfo[]> cache = new();
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Finds suitable constructors on the target type
    /// </summary>
    /// <param name="targetType">Type to search for constructors</param>
    /// <returns>Suitable found constructors</returns>
    public ConstructorInfo[] FindConstructors(Type targetType)
    {
        ConstructorInfo[] result = cache.GetOrAdd(targetType, t => t.GetTypeInfo().DeclaredConstructors.Where(c => !c.IsStatic).ToArray());
        return result.Length > 0 ? result : throw new NoConstructorsFoundException(targetType, this);
    }
    #endregion
}