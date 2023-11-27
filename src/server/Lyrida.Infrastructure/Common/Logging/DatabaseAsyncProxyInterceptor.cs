#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using Castle.DynamicProxy;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Configuration;
using Lyrida.Infrastructure.Common.Utilities;
#endregion

namespace Lyrida.Infrastructure.Common.Logging;

/// <summary>
/// Class for providing asynchronous support for the dynamic proxy interceptor of DAL
/// </summary>
/// <remarks>
/// Creation Date: 20th of June, 2021
/// </remarks>
public class DatabaseAsyncProxyInterceptor : AsyncInterceptorBase
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ILoggerManager logger;
    private readonly IConfiguration configuration;
    private readonly Dictionary<string, string> dbTableNamesMaping = new();
    private static readonly ConcurrentDictionary<Type, string> PropertyNamesCache = new();
    private const string DATA_ACCESS = "Lyrida.DataAccess";
    private const string I_STORAGE_DTO = "IStorageDto";
    private const string DTOs = "Lyrida.DataAccess.Common.DTO";
    private const string MAPS_TO_ATTRIBUTE = "Lyrida.DataAccess.Common.Attributes.MapsToAttribute, Lyrida.DataAccess";
    private const string IGNORE_ON_QUERY_ATTRIBUTE = "Lyrida.DataAccess.Common.Attributes.IgnoreOnQueryAttribute, Lyrida.DataAccess";
    private const string IGNORE_ON_COMMAND_ATTRIBUTE = "Lyrida.DataAccess.Common.Attributes.IgnoreOnCommandAttribute, Lyrida.DataAccess";
    #endregion

    #region ==================================================================== PROPERTIES =================================================================================
    public static bool UsesConsoleLogger { get; set; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="logger">The injected logger to be used in interception.</param>
    /// <param name="configuration">Injected service for application configurations.</param>
    public DatabaseAsyncProxyInterceptor(ILoggerManager logger, IConfiguration configuration)
    {
        this.logger = logger;
        this.configuration = configuration;
        MapDatabaseTableNames();
        CacheDataAccessLayerEntitiesPropertyNames();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Intercepts method invocations, both synchronous and asynchronous, that have no return type, and adds logging functionality to them.
    /// </summary>
    /// <param name="invocation">The method that will be invoked.</param>
    /// <param name="proceedInfo"> Describes the <see cref="IInvocation.Proceed"/> operation for <paramref name="invocation"/> at a specific point during interception.</param>
    /// <param name="proceed">The function to proceed the <paramref name="proceedInfo"/>.</param>
    /// <returns>A Task object that represents the asynchronous operation.</returns>
    protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
    {
        try
        {
            // method that returns a task has no return value
            await proceed(invocation, invocation.CaptureProceedInfo()).ConfigureAwait(false);
        }
        catch (Exception innerException)
        {
            // wrap the original exception in a new exception that contains information about the intercepted method
            Exception outterException =
                   new(
                       $"{invocation.Method.DeclaringType}.{invocation.Method.Name}" +
                       Environment.NewLine +
                       string.Join(", ", invocation.Arguments.Select(a => (a ?? string.Empty).ToString())),
                   innerException);
            logger.LogError(outterException);
            if (!UsesConsoleLogger)
            {
                Debug.WriteLine(outterException);
                Trace.WriteLine(outterException);
            }
            // preserve the original exception type and its stacktrace and re-throw it
            ExceptionDispatchInfo.Capture(innerException).Throw();
        }
    }

    /// <summary>
    /// Intercepts method invocations, both synchronous and asynchronous, that have a return type, and adds logging functionality to them.
    /// </summary>
    /// <typeparam name="TResult">The type of the result to return.</typeparam>
    /// <param name="invocation">The method that will be invoked.</param>
    /// <param name="proceedInfo"> Describes the <see cref="IInvocation.Proceed"/> operation for <paramref name="invocation"/> at a specific point during interception.</param>
    /// <param name="proceed">The function to proceed the <paramref name="proceedInfo"/>.</param>
    /// <returns>A Task object that represents the result of awaiting the asynchronous operation.</returns>
    protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
    {
        string query = string.Empty;
        if (invocation.Method.Name == "UpdateAsync")
        {
            query = "UPDATE " + dbTableNamesMaping[invocation.Arguments[0].ToString()!] + " SET ";
            if (invocation.Arguments[1] != null)
            {
                // cannot use lambda expressions as arguments to dynamically dispatched operations, so, declare it externally, for readability
                bool predicate(PropertyInfo e) => e.GetValue(invocation.Arguments[1]) != null;
                // get the properties of the filter object
                PropertyInfo[] parameters = invocation.Arguments[1].GetType()
                                                                   .GetProperties()
                                                                   .Where(e => predicate(e))
                                                                   .ToArray();
                int i = 0;
                // reconstruct the columns part of the string from the properties of filter, using named parameters
                foreach (PropertyInfo parameter in parameters)
                {
                    // do not update properties that were marked as non-insertable/updateble
                    Type? ignoreAttributeType = Type.GetType(IGNORE_ON_COMMAND_ATTRIBUTE);
                    if (ignoreAttributeType != null)
                    {
                        if (parameter.GetCustomAttributes(ignoreAttributeType).None())
                        {
                            // check if the property has custom name mapping to the storage container, and if so, use those instead
                            Type? mappingAttributeType = Type.GetType(MAPS_TO_ATTRIBUTE);
                            if (mappingAttributeType != null)
                            {
                                dynamic? mappedAttribute = parameter.GetCustomAttribute(mappingAttributeType);
                                var value = parameter.GetValue(invocation.Arguments[1]);
                                string columnName = (mappedAttribute?.Name ?? parameter.Name);
                                // dont include sesnsitive data
                                if (columnName.Contains("password", StringComparison.OrdinalIgnoreCase) || columnName.Contains("totp", StringComparison.OrdinalIgnoreCase))
                                    value = "not included in logs";
                                query += columnName + " = " + (value is string ? "'" : "") + value + (value is string ? "'" : "");
                                if (parameters.Length > 1 && i < parameters.Length - 1)
                                {
                                    query += ", ";
                                    i++;
                                }
                            }
                        }
                        else
                            i++;
                    }
                }
            }
            if (query.EndsWith(", "))
                query = query[0..^2];
            query += invocation.Arguments[2] != null ? " WHERE " : string.Empty;
        }
        else if (invocation.Method.Name == "SelectAsync")
        {
            if (invocation.Arguments.Length == 5) // case when the column mappings are not specified manually 
                query = "SELECT " + PropertyNamesCache.GetOrAdd(invocation.GenericArguments[0], GetProperties) + " FROM " + dbTableNamesMaping[invocation.Arguments[0].ToString()!] + (invocation.Arguments[1] != null ? " WHERE " : string.Empty);
            else // case when the column mappings are specified manually (SELECT x AS ..)
                query = "SELECT " + invocation.Arguments[1] + " FROM " + dbTableNamesMaping[invocation.Arguments[0].ToString()!] + (invocation.Arguments[2] != null ? " WHERE " : string.Empty);
        }
        else if (invocation.Method.Name == "DeleteAsync")
            query = "DELETE FROM " + dbTableNamesMaping[invocation.Arguments[0].ToString()!] + (invocation.Arguments[2] != null ? " WHERE " : string.Empty);
        else if (invocation.Method.Name == "InsertAsync")
            query = "INSERT INTO " + dbTableNamesMaping[invocation.Arguments[0].ToString()!] + " (" + GetColumns(invocation.Arguments[1]) + ") VALUES (" + GetParameters(invocation.Arguments[1]) + ");";
        else if (invocation.Method.Name == "ExecuteAsync")
        {
            // check if the query contains named parameters, and if so, substitute them with the actual values
            if (invocation.Arguments[1] != null)
                query = SubstituteNamedParameters(invocation.Arguments[0].ToString()!, invocation.Arguments[1]);
            else
                query = invocation.Arguments[0].ToString()!;
        }
        if (invocation.Method.Name != "InsertAsync" && invocation.Method.Name != "ExecuteAsync")
        {
            // "delete" and "select without manual mapping" have different place for the "where" parameters
            int filterArgumentId = invocation.Method.Name != "DeleteAsync" ? (invocation.Method.Name == "SelectAsync" && invocation.Arguments.Length == 5 ? 1 : 2) : 1;
            if (invocation.Arguments[filterArgumentId] != null)
            {
                // cannot use lambda expressions as arguments to dynamically dispatched operations, so, declare it externally, for readability
                bool predicate(PropertyInfo e) => e.GetValue(invocation.Arguments[filterArgumentId]) != null;
                // get the properties of the filter object
                PropertyInfo[] filters = invocation.Arguments[filterArgumentId].GetType()
                                               .GetProperties()
                                               .Where(e => predicate(e))
                                               .ToArray();
                int i = 0;
                // reconstruct the WHERE clause part of the string from the properties of filter, using named parameters
                foreach (PropertyInfo f in filters)
                {
                    query += f.Name + " = " + f.GetValue(invocation.Arguments[filterArgumentId]);
                    if (filters.Length > 1 && i < filters.Length - 1)
                    {
                        query += " AND ";
                        i++;
                    }
                }
            }
        }
        if (!UsesConsoleLogger)
            Trace.WriteLine("QUERY: " + query);
        // log the API endpoint invocation origin and form
        logger.LogInfo("\t" + query + Environment.NewLine +
            "\t\t\t\t\t'File was " + invocation.Arguments[^1] + "'" + Environment.NewLine +
            "\t\t\t\t\t'Method was " + invocation.Arguments[^2] + "()'" + Environment.NewLine +
            "\t\t\t\t\t'Line was " + invocation.Arguments[^3] + "'");
        try
        {
            // intercepted method is invoked here, return its awaited result
            TResult? result = await proceed(invocation, invocation.CaptureProceedInfo()).ConfigureAwait(false);
            if (result != null)
            {
                // get the "Error" property of ApiResponse
                var errorProperty = result.GetType().GetProperty("Error");
                if (errorProperty != null)
                {
                    string? errorValue = errorProperty.GetValue(result) as string;
                    // if the value of this property is not null, log it!
                    if (!string.IsNullOrEmpty(errorValue))
                    {
                        logger.LogError(errorValue);
                        if (!UsesConsoleLogger)
                            Trace.WriteLine($"Error: {errorValue}");
                    }
                }
            }
            return result;
        }
        catch (Exception innerException)
        {
            // wrap the original exception in a new exception that contains information about the intercepted method
            Exception outterException =
                new(
                    $"{invocation.Method.DeclaringType}.{invocation.Method.Name}" +
                    Environment.NewLine +
                    string.Join(", ", invocation.Arguments.Select(a => (a ?? string.Empty).ToString())),
                innerException);
            logger.LogError(outterException);
            if (!UsesConsoleLogger)
            {
                Debug.WriteLine(outterException);
                Trace.WriteLine(outterException);
            }
            // a return value MUST be set as the return value of the intercepted method, or the proxy class will throw an InvalidOperationException!
            invocation.ReturnValue = default;
            // preserve the original exception type and its stacktrace and re-throw it
            ExceptionDispatchInfo.Capture(innerException).Throw();
            throw;
        }
    }

    /// <summary>
    /// Substitutes named parameters in SQL queries with their corresponding values.
    /// </summary>
    /// <param name="originalQuery">The original SQL query.</param>
    /// <param name="data">The anonymous object containing the values of the named parameters.</param>
    /// <returns>A string representing the original SQL queries, with the named parameters replaced by their corresponding values.</returns>
    private static string SubstituteNamedParameters(string originalQuery, object data)
    {
        // get the properties of the filter object
        PropertyInfo[] filters = data.GetType()
                                       .GetProperties()
                                       .ToArray();
        // replace the named parameters with the corresponding values
        foreach (PropertyInfo f in filters)
        {
            if (originalQuery.Contains("@" + f.Name))
            {
                object? propertyValue = f.GetValue(data);
                // account for array parameters!
                originalQuery = originalQuery.Replace("@" + f.Name,
                    propertyValue is Array ? "('" + string.Join("', '", ((System.Collections.IEnumerable)propertyValue).Cast<object>().Select(i => i.ToString())) + "')" :
                    propertyValue?.ToString());
            }
        }
        return originalQuery;
    }

    /// <summary>
    /// Gets the names of the public properties of <paramref name="model"/> and formats them as a string used in SQL queries.
    /// </summary>
    /// <typeparam name="TDto">The type of <paramref name="model"/>.</typeparam>
    /// <param name="model">A database model containing the public properties used for getting or saving data in a database table.</param>
    /// <returns>A formatted string used in SQL queries, composed of the names of the public properties of <paramref name="model"/>.</returns>
    internal static string GetColumns<TDto>(TDto model)
    {
        if (model != null)
        {
            Type? mappingAttributeType = Type.GetType(MAPS_TO_ATTRIBUTE);
            Type? ignoreAttributeType = Type.GetType(IGNORE_ON_COMMAND_ATTRIBUTE);
            // get a list of all public properties of the provided model, but ignore those with the IgnoreOnCommand attribute
            IEnumerable<PropertyInfo> properties = model.GetType()
                                                        .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                                                        .Where(e => e.GetCustomAttributes(ignoreAttributeType!)
                                                        .None());
            // concatenate all the above properties in a single string, separated by comas, and escape them as SQL column names
            // in case the properties have custom names in the storage container, get that name from their mapping attribute

            string result = string.Empty;
            foreach (PropertyInfo property in properties)
            {
                dynamic mappedAttribute = property.GetCustomAttribute(mappingAttributeType!)!;
                if (mappedAttribute != null && ((IEnumerable<dynamic>)mappedAttribute!.GetType().GetProperties()).Any(p => p.Name == "Name"))
                    result += (((IEnumerable<dynamic>)mappedAttribute!.GetType().GetProperties()).First(p => p.Name == "Name")).GetValue(mappedAttribute) + " AS " + property.Name + ", ";
                else
                    result += property.Name + ", ";
            }
            if (result.EndsWith(", "))
                result = result[..result.LastIndexOf(", ")];
            return result;
        }
        else
            throw new ArgumentException("The model cannot be empty!");
    }

    /// <summary>
    /// Gets the values of the public properties of <paramref name="model"/> and formats them as a string used in SQL queries.
    /// </summary>
    /// <typeparam name="TDto">The type of <paramref name="model"/>.</typeparam>
    /// <param name="model">A database model containing the public properties whose values are used for saving data in a database table.</param>
    /// <returns>A formatted string used in SQL queries, composed of the columns of the public properties of <paramref name="model"/>.</returns>
    internal static string GetParameters<TDto>(TDto model)
    {
        if (model != null)
        {
            Type? ignoreAttributeType = Type.GetType(IGNORE_ON_COMMAND_ATTRIBUTE);
            // get a list of all public properties of the provided model, but ignore those with the IgnoreOnCommand attribute
            IEnumerable<PropertyInfo> properties = model.GetType()
                                                        .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                                                        .Where(e => e.GetCustomAttributes(ignoreAttributeType!)
                                                        .None());
            return string.Join(", ", properties.Select(e =>
            {
                // dont include sesnsitive data
                if (e.Name.Contains("password", StringComparison.OrdinalIgnoreCase) || e.Name.Contains("totp", StringComparison.OrdinalIgnoreCase))
                    return "not included in logs";
                var value = e.GetValue(model);
                if (value is null)
                    return "NULL";
                else if (value is string stringVal)
                {
                    if (!string.IsNullOrEmpty(stringVal))
                        return "'" + stringVal + "'";
                    else
                        return "NULL";
                }
                else
                    return value;
            }));
        }
        else
            throw new ArgumentException("The model cannot be empty!");
    }

    /// <summary>
    /// Gets the values of the public properties of <paramref name="type"/> and formats them as a string used in SQL queries.
    /// </summary>
    /// <param name="type">A database element containing the public properties whose values are used for saving data in a database table.</param>
    /// <returns>A formatted string used in SQL queries, composed of the columns of the public properties of <paramref name="type"/>.</returns>
    internal static string GetProperties(Type type) 
    {
        var ignoreOnQueryAttributeType = Type.GetType(IGNORE_ON_QUERY_ATTRIBUTE);
        var properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                             .Where(property => property.GetCustomAttributes(ignoreOnQueryAttributeType!, false).None());
        var propertyNames = new List<string>();

        var mapsToAttributeType = Type.GetType(MAPS_TO_ATTRIBUTE);
        var nameProperty = mapsToAttributeType?.GetProperty("Name");
        // for each property of the element, either get the value of its MapsToAttribute's Name property (if it has this attribute) or its name (if it doesn't)
        foreach (var property in properties)
        {
            var mapsToAttribute = property.GetCustomAttributes(mapsToAttributeType!, false).FirstOrDefault();
            if (mapsToAttribute != null && nameProperty != null)
            {
                var name = nameProperty.GetValue(mapsToAttribute) as string;
                if (!string.IsNullOrEmpty(name))
                {
                    propertyNames.Add(name + " AS " + property.Name);
                    continue;
                }
            }
            propertyNames.Add(property.Name);
        }
        return string.Join(", ", propertyNames);
    }

    /// <summary>
    /// Caches the Data Access Layer DTO types, for performance improved access.
    /// </summary>
    private static void CacheDataAccessLayerEntitiesPropertyNames()
    {
        var assembly = Assembly.Load(DATA_ACCESS);
        var types = assembly.GetTypes()
                            .Where(t => t.Namespace != null && t.Namespace.StartsWith(DTOs) && t.GetInterfaces()
                                                                                                .Any(i => i.Name == I_STORAGE_DTO));
        foreach (var type in types)
            PropertyNamesCache.TryAdd(type, GetProperties(type));
    }

    /// <summary>
    /// Maps the names of the database columns to the names of the columns used in application.
    /// </summary>
    private void MapDatabaseTableNames()
    {
        dbTableNamesMaping.Add("Users", "users");
        dbTableNamesMaping.Add("UserPages", "userpages");
        dbTableNamesMaping.Add("UserPreferences", "userpreferences");
        dbTableNamesMaping.Add("UserEnvironments", "userenvironments");
    }
    #endregion
}