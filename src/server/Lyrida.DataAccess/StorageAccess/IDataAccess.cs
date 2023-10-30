#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Lyrida.DataAccess.Common.Enums;
using System.Runtime.CompilerServices;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.StorageAccess;

/// <summary>
/// Interface for getting or saving data to and from a generic storage medium
/// </summary>
/// <remarks>
/// Creation Date: 12th of June, 2021
/// </remarks>
public interface IDataAccess
{

    #region ==================================================================== PROPERTIES =================================================================================
    public Func<string?>? ConnectionStringFactory { get; set; }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Opens a transaction
    /// </summary>
    void OpenTransaction();

    /// <summary>
    /// Closes a transaction, rolls back changes if the transaction was faulty
    /// </summary>
    void CloseTransaction();

    /// <summary>
    /// Executes a generic operation on the storage medium
    /// </summary>
    /// <typeparam name="TDto">The type of the model to get from the storage medium as a result of <paramref name="query"/></typeparam>
    /// <param name="query">The command to execute on the storage medium</param>
    /// <param name="param">The prameters of <paramref name="query"/>, if any</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse{TDto}"/> instance containing the requested data from the storage medium, if any, or the provided error, in case of failure</returns>
    Task<ApiResponse<TDto>> ExecuteAsync<TDto>(string query, dynamic? param = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) where TDto : IStorageDto;

    /// <summary>
    /// Executes a generic operation on the storage medium
    /// </summary>
    /// <param name="query">The command to execute on the storage medium</param>
    /// <param name="filter">Specifies an anonymous object whose properties are used for the named parameters</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse"/> instance containing the affected number of rows, or the provided error, in case of failure</returns>
    Task<ApiResponse> ExecuteAsync(string query, object? filter = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null);

    /// <summary>
    /// Deletes data from the storage medium
    /// </summary>
    /// <param name="container">The storage container from which to delete data</param>
    /// <param name="filter">Used for conditional deletes, specifies an object whose properties are used for the conditions</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse"/> instance containing the count of affected entries, or the provided error, in case of failure</returns>
    Task<ApiResponse> DeleteAsync(DataContainers container, dynamic? filter = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null);

    /// <summary>
    /// Updates data in the database using an anonymous model
    /// </summary>
    /// <param name="container">The container in which to update the data</param>
    /// <param name="values">An object whose properties are used for the columns to be updated</param>
    /// <param name="filter">Used for conditional selects, specifies an object whose properties are used for the conditions (SELECT ... FROM ... WHERE ...)</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse"/> instance containing the count of affected rows in the database, or the provided error, in case of failure</returns>
    Task<ApiResponse> UpdateAsync(DataContainers container, dynamic values, dynamic? filter = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null);

    /// <summary>
    /// Updates data in the database using a model
    /// </summary>
    /// <typeparam name="TDto">The type of the model to update in the database</typeparam>
    /// <param name="container">The table in which to update the data</param>
    /// <param name="model">The model to be updated</param>
    /// <param name="filter">Used for conditional selects, specifies an object whose properties are used for the conditions (SELECT ... FROM ... WHERE ...)</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse"/> instance containing the count of affected rows in the database, or the provided error, in case of failure</returns>
    Task<ApiResponse> UpdateAsync<TDto>(DataContainers container, TDto model, object? filter = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) where TDto : IStorageDto, new();

    /// <summary>
    /// Saves data of type <typeparamref name="TDto"/> in the storage medium
    /// </summary>
    /// <typeparam name="TDto">The type of the model to be saved</typeparam>
    /// <param name="container">The storage container in which to insert data</param>
    /// <param name="model">The model to be saved</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse{TDto}"/> instance containing the id of the inserted data, or the provided error, in case of failure</returns>
    Task<ApiResponse<TDto>> InsertAsync<TDto>(DataContainers container, TDto model, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) where TDto : IStorageDto, new();

    /// <summary>
    /// Selects data of type <typeparamref name="TDto"/> from the storage medium with manual mapping
    /// </summary>
    /// <typeparam name="TDto">The type of the model to get from the storage medium</typeparam>
    /// <param name="container">The storage container from which to select the data</param>
    /// <param name="columns">The columns to take from <paramref name="container"/></param>
    /// <param name="filter">Used for conditional selects, specifies an object whose properties are used for the conditions</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse{TDto}"/> instance containing the requested data from the storage medium, or the provided error, in case of failure</returns>
    Task<ApiResponse<TDto>> SelectAsync<TDto>(DataContainers container, string? columns, dynamic? filter = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) where TDto : IStorageDto;

    /// <summary>
    /// Selects data of type <typeparamref name="TDto"/> from the storage medium without manual mapping
    /// </summary>
    /// <typeparam name="TDto">The type of the model to get from the storage medium</typeparam>
    /// <param name="container">The storage container from which to select the data</param>
    /// <param name="filter">Used for conditional selects, specifies an object whose properties are used for the conditions</param>
    /// <param name="line">The line number in the source file where the method is called</param>
    /// <param name="caller">The method or property name of the caller of the method</param>
    /// <param name="file">The full path of the file that contains the caller, at compile time</param>
    /// <returns>An <see cref="ApiResponse{TDto}"/> instance containing the requested data from the storage medium, or the provided error, in case of failure</returns>
    Task<ApiResponse<TDto>> SelectAsync<TDto>(DataContainers container, dynamic? filter = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) where TDto : IStorageDto;
    #endregion
}