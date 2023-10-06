#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
#endregion

namespace Lyrida.Api.Common.ModelBinders;

/// <summary>
/// Custom model binder for URL parameters. For some reason, the default binder converts "%20" to "+", instead of " "
/// </summary>
/// <remarks>
/// Creation Date: 01st of October, 2023
/// </remarks>
public class UrlStringBinder : IModelBinder
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Binds a model to a value by using the specified controller context and binding context.
    /// </summary>
    /// <param name="bindingContext">The context within which to bind the model.</param>
    /// <returns>A task representing the bind operation.</returns>
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));
        string modelName = bindingContext.ModelName;
        // try to fetch the value of the argument by name
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;
        string? value = valueProviderResult.FirstValue;
        if (value is null)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }
        // do the custom decoding here
        value = Uri.UnescapeDataString(value);
        bindingContext.Result = ModelBindingResult.Success(value);
        return Task.CompletedTask;
    }
    #endregion
}