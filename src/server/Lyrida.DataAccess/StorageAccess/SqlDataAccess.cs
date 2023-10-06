#region ========================================================================= USING =====================================================================================
using Dapper;
using System;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Lyrida.DataAccess.Common.Enums;
using System.Runtime.CompilerServices;
using Lyrida.DataAccess.Common.Attributes;
using Lyrida.Infrastructure.Common.Utilities;
using Lyrida.DataAccess.Common.Entities.Common;
#endregion

namespace Lyrida.DataAccess.StorageAccess;

/// <summary>
/// Gets or saves data to and from an ORM database 
/// </summary>
/// <remarks>
/// Creation Date: 02nd of December, 2020
/// </remarks>
internal sealed class SqlDataAccess : IDataAccess
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private bool isTransactionFaulty;
    internal static bool usesSnakeCasingDatabaseNaming = true;
    private IDbTransaction? dbTransaction;
    private readonly IDbConnection dbConnection;
    private readonly Dictionary<EntityContainers, string> dbTableNamesMaping = new();
    #endregion

    #region ==================================================================== PROPERTIES =================================================================================
    public Func<string?>? ConnectionStringFactory { get; set; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="dbConnection">The injected database connection to use</param>
    internal SqlDataAccess(IDbConnection dbConnection)
    {
        this.dbConnection = dbConnection;
        MapDatabaseTableNames();
        // set timout to 2 minutes (some queries are heavy)
        SqlMapper.Settings.CommandTimeout = 120;
        // map the string type to the bool database type (save booleans as strings, not integers)
        SqlMapper.AddTypeMap(typeof(bool), DbType.String);
        SqlMapper.AddTypeMap(typeof(bool?), DbType.String);
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    /// <summary>
    /// Default Destructor
    /// </summary>
    ~SqlDataAccess()
    {
        dbConnection?.Dispose();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Executes a generic operation on the storage medium
    /// </summary>
    /// <typeparam name="TEntity">The type of the model to get from the storage medium as a result of <paramref name="command"/></typeparam>
    /// <param name="query">The command to execute on the storage medium</param>
    /// <param name="filter">Specifies an anonymous object whose properties are used for the named parameters</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse{TEntity}"/> instance containing the requested data from the storage medium, if any, or the provided error, in case of failure</returns>
    public async Task<ApiResponse<TEntity>> ExecuteAsync<TEntity>(string query, object? filter = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) where TEntity : IStorageEntity
    {
        ApiResponse<TEntity> serializedData = new();
        try
        {
            if (dbTransaction == null && dbConnection.State != ConnectionState.Open)
                dbConnection.ConnectionString = ConnectionStringFactory?.Invoke();
            // execute the SQL query
            TEntity[] temp = (await dbConnection.QueryAsync<TEntity>(query, filter ?? null)).ToArray();
            // construct the API response and return it
            serializedData.Data = temp.Length > 0 ? temp : null;
            serializedData.Count = temp.Length;
        }
        catch (Exception ex)
        {
            // mark the transaction as faulty, so it is rolled back
            isTransactionFaulty = true;
            CloseTransaction();
            // in case of exceptions, populate the Error property of the API response
            serializedData.Error = ex.AggregateMessages();
        }
        return serializedData;
    }

    /// <summary>
    /// Executes a generic operation on the storage medium
    /// </summary>
    /// <param name="query">The command to execute on the storage medium</param>
    /// <param name="filter">Specifies an anonymous object whose properties are used for the named parameters</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse"/> instance containing the affected number of rows, or the provided error, in case of failure</returns>
    public async Task<ApiResponse> ExecuteAsync(string query, object? filter = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null)
    {
        ApiResponse serializedData = new();
        try
        {
            if (dbTransaction == null && dbConnection.State != ConnectionState.Open)
                dbConnection.ConnectionString = ConnectionStringFactory?.Invoke();
            // execute the SQL query
            var temp = (await dbConnection.QueryAsync(query, filter ?? null)).ToArray();
            // construct the API response and return it
            serializedData.Count = temp.Length;
        }
        catch (Exception ex)
        {
            // mark the transaction as faulty, so it is rolled back
            isTransactionFaulty = true;
            CloseTransaction();
            // in case of exceptions, populate the Error property of the API response
            serializedData.Error = ex.AggregateMessages();
        }
        return serializedData;
    }

    /// <summary>
    /// Selects data of type <typeparamref name="TEntity"/> from the database with manual column mapping
    /// </summary>
    /// <typeparam name="TEntity">The type of the model to get from the database</typeparam>
    /// <param name="table">The table from which to select the data</param>
    /// <param name="columns">The columns to take from <paramref name="table"/></param>
    /// <param name="filter">Used for conditional selects, specifies an object whose properties are used for the conditions (SELECT ... FROM ... WHERE ...)</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse{TEntity}"/> instance containing the requested data from the database, or the provided error, in case of failure</returns>
    public async Task<ApiResponse<TEntity>> SelectAsync<TEntity>(EntityContainers table, string? columns, object? filter = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) where TEntity : IStorageEntity
    {
        ApiResponse<TEntity> serializedData = new();
        try
        {
            if (dbTransaction == null && dbConnection.State != ConnectionState.Open)
                dbConnection.ConnectionString = ConnectionStringFactory?.Invoke();
            // reconstruct the SQL query from the parameters
            string query = "SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + dbTableNamesMaping[table] + (filter != null ? " WHERE " : string.Empty);
            // if a filter was specified, add the properties and their values from filter as the WHERE clauses of the SQL query
            Dictionary<string, object>? args = null;
            if (filter != null)
            {
                // cannot use lambda expressions as arguments to dynamically dispatched operations, so, declare it externally, for readability
                bool predicate(PropertyInfo e) => e.GetValue(filter) != null;
                // get the properties of the filter object
                PropertyInfo[] filters = filter.GetType()
                                               .GetProperties()
                                               .Where(e => predicate(e))
                                               .ToArray();
                args = new Dictionary<string, object>();
                int i = 0;
                // reconstruct the WHERE clause part of the string from the properties of filter, using named parameters
                foreach (PropertyInfo f in filters)
                {
                    query += f.Name + " = @" + f.Name;
                    if (!args.ContainsKey("@" + f.Name))
                        args.Add("@" + f.Name, f.GetValue(filter)!);
                    if (filters.Length > 1 && i < filters.Length - 1)
                    {
                        query += " AND ";
                        i++;
                    }
                }
            }
            query += ";";
            // execute the SQL query
            TEntity[] temp = (await dbConnection.QueryAsync<TEntity>(query, args ?? null)).ToArray();
            // construct the API response and return it
            serializedData.Data = temp.Length > 0 ? temp : null;
            serializedData.Count = temp.Length;
        }
        catch (Exception ex)
        {
            // mark the transaction as faulty, so it is rolled back
            isTransactionFaulty = true;
            CloseTransaction();
            // in case of exceptions, populate the Error property of the API response
            serializedData.Error = ex.AggregateMessages();
            Trace.WriteLine(ex.Message);
        }
        return serializedData;
    }

    /// <summary>
    /// Selects data of type <typeparamref name="TEntity"/> from the database without manual column mapping
    /// </summary>
    /// <typeparam name="TEntity">The type of the model to get from the database</typeparam>
    /// <param name="table">The table from which to select the data</param>
    /// <param name="filter">Used for conditional selects, specifies an object whose properties are used for the conditions (SELECT ... FROM ... WHERE ...)</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse{TEntity}"/> instance containing the requested data from the database, or the provided error, in case of failure</returns>
    public async Task<ApiResponse<TEntity>> SelectAsync<TEntity>(EntityContainers table, object? filter = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) where TEntity : IStorageEntity
    {
        ApiResponse<TEntity> serializedData = new();
        try
        {
            if (dbTransaction == null && dbConnection.State != ConnectionState.Open)
                dbConnection.ConnectionString = ConnectionStringFactory?.Invoke();
            // reconstruct the SQL query from the parameters
            string query = "SELECT " + GetProperties<TEntity>() + " FROM " + dbTableNamesMaping[table] + (filter != null ? " WHERE " : string.Empty);
            // if a filter was specified, add the properties and their values from filter as the WHERE clauses of the SQL query
            Dictionary<string, object>? args = null;
            if (filter != null)
            {
                // cannot use lambda expressions as arguments to dynamically dispatched operations, so, declare it externally, for readability
                bool predicate(PropertyInfo e) => e.GetValue(filter) != null;
                // get the properties of the filter object
                PropertyInfo[] filters = filter.GetType()
                                               .GetProperties()
                                               .Where(e => predicate(e))
                                               .ToArray();
                args = new Dictionary<string, object>();
                int i = 0;
                // reconstruct the WHERE clause part of the string from the properties of filter, using named parameters
                foreach (PropertyInfo f in filters)
                {
                    query += f.Name + " = @" + f.Name;
                    if (!args.ContainsKey("@" + f.Name))
                        args.Add("@" + f.Name, f.GetValue(filter)!);
                    if (filters.Length > 1 && i < filters.Length - 1)
                    {
                        query += " AND ";
                        i++;
                    }
                }
            }
            query += ";";
            // execute the SQL query
            TEntity[] temp = (await dbConnection.QueryAsync<TEntity>(query, args ?? null)).ToArray();
            // construct the API response and return it
            serializedData.Data = temp.Length > 0 ? temp : null;
            serializedData.Count = temp.Length;
        }
        catch (Exception ex)
        {
            // mark the transaction as faulty, so it is rolled back
            isTransactionFaulty = true;
            CloseTransaction();
            // in case of exceptions, populate the Error property of the API response
            serializedData.Error = ex.AggregateMessages();
            Trace.WriteLine(ex.Message);
        }
        return serializedData;
    }

    /// <summary>
    /// Saves data of type <typeparamref name="TEntity"/> in the database
    /// </summary>
    /// <typeparam name="TEntity">The type of the model to save in the database</typeparam>
    /// <param name="table">The table in which to insert data</param>
    /// <param name="model">The model to be saved</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse{TEntity}"/> instance containing the id of the inserted data from the database, or the provided error, in case of failure</returns>
    public async Task<ApiResponse<TEntity>> InsertAsync<TEntity>(EntityContainers table, TEntity model, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) where TEntity : IStorageEntity, new()
    {
        ApiResponse<TEntity> serializedData = new();
        try
        {
            if (dbTransaction == null && dbConnection.State != ConnectionState.Open)
                dbConnection.ConnectionString = ConnectionStringFactory?.Invoke();
            // reconstruct the SQL query from the properties of the provided model
            string query = "INSERT INTO " + dbTableNamesMaping[table] + " (\n\t" + GetColumns(model) + ")\nVALUES (\n\t" + GetParameters(model) + ");";
            // also select last inserted row id
            if (dbConnection.GetType().Name == "MySqlConnection")
                query += "\nSELECT last_insert_id();";
#if DEBUG
            //Debug.WriteLine(query);
#endif
            // execute the SQL query and construct the API response with the returned id of the inserted data
            serializedData.Data = new TEntity[] { new TEntity() { Id = await dbConnection.ExecuteScalarAsync<int>(query, model) } };
            serializedData.Count = 1;
        }
        catch (Exception ex)
        {
            // mark the transaction as faulty, so it is rolled back
            isTransactionFaulty = true;
            CloseTransaction();
            // in case of exceptions, populate the Error property of the API response
            serializedData.Error = ex.AggregateMessages();
            Trace.WriteLine(ex.Message);
        }
        return serializedData;
    }

    /// <summary>
    /// Updates data in the database using a model
    /// </summary>
    /// <typeparam name="TEntity">The type of the model to update in the database</typeparam>
    /// <param name="table">The table in which to update the data</param>
    /// <param name="model">The model to be updated</param>
    /// <param name="filter">Used for conditional selects, specifies an object whose properties are used for the conditions (SELECT ... FROM ... WHERE ...)</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse"/> instance containing the count of affected rows in the database, or the provided error, in case of failure</returns>
    public async Task<ApiResponse> UpdateAsync<TEntity>(EntityContainers table, TEntity model, object? filter = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) where TEntity : IStorageEntity, new()
    {
        ApiResponse serializedData = new();
        try
        {
            if (dbTransaction == null && dbConnection.State != ConnectionState.Open)
                dbConnection.ConnectionString = ConnectionStringFactory?.Invoke();
            // reconstruct the SQL query from the parameters
            string query = "UPDATE " + dbTableNamesMaping[table] + " SET ";
            Dictionary<string, object>? args = null;
            // set the SQL update named parameters
            // cannot use lambda expressions as arguments to dynamically dispatched operations, so, declare it externally, for readability
            //bool predicate(PropertyInfo e) => e.GetValue(model) != null;
            // get the properties of the filter object
            PropertyInfo[] parameters = model.GetType()
                                             .GetProperties()
                                             //.Where(e => predicate(e))
                                             .ToArray();
            args ??= new Dictionary<string, object>();
            int i = 0;
            // reconstruct the columns part of the string from the properties of filter, using named parameters
            foreach (PropertyInfo parameter in parameters)
            {
                // do not update properties that were marked as non-insertable/updateble
                if (!parameter.GetCustomAttributes<IgnoreOnCommandAttribute>().Any())
                {
                    // check if the property has custom name mapping to the storage container, and if so, use those instead
                    query += (parameter.GetCustomAttribute<MapsToAttribute>()?.Name ?? parameter.Name) + " = @" + parameter.Name;
                    if (!args.ContainsKey("@" + parameter.Name))
                        args.Add("@" + parameter.Name, parameter.GetValue(model)!);
                    if (parameters.Length > 1 && i < parameters.Length - 1)
                    {
                        query += ", ";
                        i++;
                    }
                }
                else
                    i++;
            }
            if (query.EndsWith(", "))
                query = query[0..^2];
            // if a filter was specified, add the properties and their values from filter as the WHERE clauses of the SQL query
            if (filter != null)
            {
                query += " WHERE ";
                // cannot use lambda expressions as arguments to dynamically dispatched operations, so, declare it externally, for readability
                bool filterPredicate(PropertyInfo e) => e.GetValue(filter) != null;
                // get the properties of the filter object
                parameters = filter.GetType()
                                .GetProperties()
                                .Where(e => filterPredicate(e))
                                .ToArray();
                args ??= new Dictionary<string, object>();
                i = 0;
                // reconstruct the WHERE clause part of the string from the properties of filter, using named parameters
                foreach (PropertyInfo f in parameters)
                {
                    query += f.Name + " = @" + f.Name;
                    if (!args.ContainsKey("@" + f.Name))
                        args.Add("@" + f.Name, f.GetValue(filter)!);
                    if (parameters.Length > 1 && i < parameters.Length - 1)
                    {
                        query += " AND ";
                        i++;
                    }
                }
            }
            else
            {
                query += " WHERE id = @id";
                args.Add("@id", model.Id);
            }
            query += ";";
#if DEBUG
            //Debug.WriteLine(query);
#endif
            // execute the SQL query and construct the API response with the number of rows affected
            serializedData.Count = await dbConnection.ExecuteAsync(query, args ?? null);
        }
        catch (Exception ex)
        {
            // mark the transaction as faulty, so it is rolled back
            isTransactionFaulty = true;
            CloseTransaction();
            // in case of exceptions, populate the Error property of the API response
            serializedData.Error = ex.AggregateMessages();
            Trace.WriteLine(ex.Message);
        }
        return serializedData;
    }

    /// <summary>
    /// Updates data in the database using an anonymous model
    /// </summary>
    /// <param name="table">The table in which to update the data</param>
    /// <param name="values">An object whose properties are used for the columns to be updated</param>
    /// <param name="filter">Used for conditional selects, specifies an object whose properties are used for the conditions (SELECT ... FROM ... WHERE ...)</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse"/> instance containing the count of affected rows in the database, or the provided error, in case of failure</returns>
    public async Task<ApiResponse> UpdateAsync(EntityContainers table, object values, object? filter = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null)
    {
        ApiResponse serializedData = new();
        try
        {
            if (dbTransaction == null && dbConnection.State != ConnectionState.Open)
                dbConnection.ConnectionString = ConnectionStringFactory?.Invoke();
            // reconstruct the SQL query from the parameters
            string query = "UPDATE " + dbTableNamesMaping[table] + " SET ";
            Dictionary<string, object>? args = null;
            // set the SQL update named parameters
            if (values != null)
            {
                // cannot use lambda expressions as arguments to dynamically dispatched operations, so, declare it externally, for readability
                bool predicate(PropertyInfo e) => e.GetValue(values) != null;
                // get the properties of the filter object
                PropertyInfo[] filters = values.GetType()
                                               .GetProperties()
                                               .Where(e => predicate(e))
                                               .ToArray();
                args ??= new Dictionary<string, object>();
                int i = 0;
                // reconstruct the columns part of the string from the properties of filter, using named parameters
                foreach (PropertyInfo f in filters)
                {
                    // check if the property has custom name mapping to the storage container, and if so, use those instead
                    query += (f.GetCustomAttribute<MapsToAttribute>()?.Name ?? f.Name) + " = @" + (f.GetCustomAttribute<MapsToAttribute>()?.Name ?? f.Name);
                    if (!args.ContainsKey("@" + f.Name))
                        args.Add("@" + f.Name, f.GetValue(values)!);
                    if (filters.Length > 1 && i < filters.Length - 1)
                    {
                        query += ", ";
                        i++;
                    }
                }
            }
            // if a filter was specified, add the properties and their values from filter as the WHERE clauses of the SQL query
            if (filter != null)
            {
                query += " WHERE ";
                // cannot use lambda expressions as arguments to dynamically dispatched operations, so, declare it externally, for readability
                bool predicate(PropertyInfo e) => e.GetValue(filter) != null;
                // get the properties of the filter object
                PropertyInfo[] filters = filter.GetType()
                                              .GetProperties()
                                              .Where(e => predicate(e))
                                              .ToArray();
                args ??= new Dictionary<string, object>();
                int i = 0;
                // reconstruct the WHERE clause part of the string from the properties of filter, using named parameters
                foreach (PropertyInfo f in filters)
                {
                    query += f.Name + " = @" + f.Name;
                    if (!args.ContainsKey("@" + f.Name))
                        args.Add("@" + f.Name, f.GetValue(filter)!);
                    if (filters.Length > 1 && i < filters.Length - 1)
                    {
                        query += " AND ";
                        i++;
                    }
                }
            }
            query += ";";
#if DEBUG
            //Debug.WriteLine(query);
#endif
            // execute the SQL query and construct the API response with the number of rows affected
            serializedData.Count = await dbConnection.ExecuteAsync(query, args ?? null);
        }
        catch (Exception ex)
        {
            // mark the transaction as faulty, so it is rolled back
            isTransactionFaulty = true;
            CloseTransaction();
            // in case of exceptions, populate the Error property of the API response
            serializedData.Error = ex.AggregateMessages();
            Trace.WriteLine(ex.Message);
        }
        return serializedData;
    }

    /// <summary>
    /// Deletes data from the database
    /// </summary>
    /// <param name="table">The table from which to delete data</param>
    /// <param name="filter">Used for conditional deletes, specifies an object whose properties are used for the conditions (DELETE FROM ... WHERE ...)</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse"/> instance containing the count of affected rows in the database, or the provided error, in case of failure</returns>
    public async Task<ApiResponse> DeleteAsync(EntityContainers table, object? filter = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null)
    {
        if (dbTransaction == null && dbConnection.State != ConnectionState.Open)
            dbConnection.ConnectionString = ConnectionStringFactory?.Invoke();
        ApiResponse serializedData = new();
        try
        {
            // reconstruct the SQL query from the parameters
            string query = "DELETE FROM " + dbTableNamesMaping[table] + (filter != null ? " WHERE " : string.Empty);
            Dictionary<string, object>? args = null;
            // if a filter was specified, add the properties and their values from filter as the WHERE clauses of the SQL query
            if (filter != null)
            {
                // cannot use lambda expressions as arguments to dynamically dispatched operations, so, declare it externally, for readability
                bool predicate(PropertyInfo e) => e.GetValue(filter) != null;
                // get the properties of the filter object
                PropertyInfo[] filters = filter.GetType()
                                               .GetProperties()
                                               .Where(e => predicate(e))
                                               .ToArray();
                args = new Dictionary<string, object>();
                int i = 0;
                // reconstruct the WHERE clause part of the string from the properties of filter, using named parameters
                foreach (PropertyInfo f in filters)
                {
                    query += f.Name + " = @" + f.Name;
                    if (!args.ContainsKey("@" + f.Name))
                        args.Add("@" + f.Name, f.GetValue(filter)!);
                    if (filters.Length > 1 && i < filters.Length - 1)
                    {
                        query += " AND ";
                        i++;
                    }
                }
            }
            query += ";";
#if DEBUG
            //Debug.WriteLine(query);
#endif
            // execute the SQL query and construct the API response with the number of rows affected
            serializedData.Count = await dbConnection.ExecuteAsync(query, args ?? null);
        }
        catch (Exception ex)
        {
            // mark the transaction as faulty, so it is rolled back
            isTransactionFaulty = true;
            CloseTransaction();
            // in case of exceptions, populate the Error property of the API response
            serializedData.Error = ex.AggregateMessages();
            Trace.WriteLine(ex.Message);
        }
        return serializedData;
    }

    /// <summary>
    /// Gets the names of the public properties of <paramref name="model"/> and formats them as a string used in SQL queries
    /// </summary>
    /// <typeparam name="TEntity">The type of <paramref name="model"/></typeparam>
    /// <param name="model">A database model containing the public properties used for getting or saving data in a database table</param>
    /// <returns>A formatted string used in SQL queries, composed of the names of the public properties of <paramref name="model"/></returns>
    internal static string GetColumns<TEntity>(TEntity model) where TEntity : IStorageEntity
    {
        if (model != null)
        {
            // get a list of all public properties of the provided model, but ignore those with the IgnoreOnCommand attribute
            IEnumerable<PropertyInfo> properties = model.GetType()
                                                        .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                                                        .Where(property => !property.GetCustomAttributes<IgnoreOnCommandAttribute>().Any());
            // concatenate all the above properties in a single string, separated by comas, and escape them as SQL column names
            // in case the properties have custom names in the storage container, get that name from their mapping attribute
            return string.Join(",\n\t", properties.Select(p => "`" + (p.GetCustomAttribute<MapsToAttribute>()?.Name ?? p.Name) + "`"));
        }
        else
            throw new ArgumentException("The model cannot be empty!");
    }

    /// <summary>
    /// Gets the values of the public properties of <paramref name="model"/> and formats them as a string used in SQL queries
    /// </summary>
    /// <typeparam name="TEntity">The type of <paramref name="model"/></typeparam>
    /// <param name="model">A database model containing the public properties whose values are used for saving data in a database table</param>
    /// <returns>A formatted string used in SQL queries, composed of the columns of the public properties of <paramref name="model"/></returns>
    internal static string GetParameters<TEntity>(TEntity model) where TEntity : IStorageEntity
    {
        if (model != null)
        {
            // get a list of all public properties of the provided model, but ignore those with the IgnoreOnCommand attribute
            IEnumerable<PropertyInfo> properties = model.GetType()
                                                        .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                                                        .Where(property => !property.GetCustomAttributes<IgnoreOnCommandAttribute>().Any());
            // concatenate all the above properties in a single string, separated by comas, and escape them as SQL column parameters
            return string.Join(",\n\t", properties.Select(p => "@" + p.Name));
        }
        else
            throw new ArgumentException("The model cannot be empty!");
    }

    /// <summary>
    /// Gets the values of the public properties of <paramref name="model"/> and formats them as a string used in SQL queries
    /// </summary>
    /// <typeparam name="TEntity">The type of <paramref name="model"/></typeparam>
    /// <param name="model">A database model containing the public properties whose values are used for saving data in a database table</param>
    /// <returns>A formatted string used in SQL queries, composed of the columns of the public properties of <paramref name="model"/></returns>
    internal static string GetProperties<TEntity>() where TEntity : IStorageEntity
    {
        // get a list of all public properties of the provided model, but ignore those with the IgnoreOnQueryAttribute attribute
        IEnumerable<PropertyInfo> properties = typeof(TEntity).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                                                              .Where(property => property.GetCustomAttributes<IgnoreOnQueryAttribute>().None());
            // concatenate all the above properties in a single string, separated by comas, and escape them as SQL column parameters
            if (usesSnakeCasingDatabaseNaming) // also, convert pascal casing to snake casing 
                return Regex.Replace(string.Join(", ", properties.Select(p => p.Name)), @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
            else
                return string.Join(", ", properties.Select(p => p.Name));
    }

    /// <summary>
    /// Maps the names of the database columns to the names of the columns used in application
    /// </summary>
    private void MapDatabaseTableNames()
    {
        dbTableNamesMaping.Add(EntityContainers.Users, "Users");
        dbTableNamesMaping.Add(EntityContainers.Roles, "Roles");
        dbTableNamesMaping.Add(EntityContainers.UserRoles, "UserRoles");
        dbTableNamesMaping.Add(EntityContainers.Permissions, "Permissions");
        dbTableNamesMaping.Add(EntityContainers.RolePermissions, "RolePermissions");
        dbTableNamesMaping.Add(EntityContainers.UserPermissions, "UserPermissions");
    }

    /// <summary>
    /// Opens an SQL transaction
    /// </summary>
    public void OpenTransaction()
    {
        if (dbTransaction == null && dbConnection.State != ConnectionState.Open)
            dbConnection.ConnectionString = ConnectionStringFactory?.Invoke();
        // check if the database connection was previously opened, and it not, open it
        if (dbConnection.State == ConnectionState.Closed)
            dbConnection.Open();
        if (dbTransaction == null) // begin the transaction
            dbTransaction = dbConnection.BeginTransaction();
        else
            throw new InvalidOperationException("A transaction is already opened!");
        Trace.WriteLine("Transaction opened!");
    }

    /// <summary>
    /// Closes an SQL transaction, rolls back changes if the transaction was faulty
    /// </summary>
    public void CloseTransaction()
    {
        // try and commit the transaction changes, if it's not marked as faulty
        try
        {
            if (!isTransactionFaulty)
                dbTransaction?.Commit();
            else
            {
                isTransactionFaulty = false;
                dbTransaction?.Rollback();
            }
            Trace.WriteLine("Transaction closed!");
        }
        catch (Exception ex)
        {
            // if the transaction failed, rollback the changes
            dbTransaction?.Rollback();
            Trace.WriteLine(ex.Message);
        }
        finally
        {
            dbTransaction?.Dispose();
            dbTransaction = null;
        }
    }
    #endregion
}