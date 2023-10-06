#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace Lyrida.Application.Common.Behaviors;

/// <summary>
/// Behavior pipeline for the register command
/// </summary>
/// <remarks>
/// Creation Date: 19th of July, 2023
/// </remarks>
/// <typeparam name="TRequest">The type representing a request. It should implement <see cref="IRequest{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The type representing a response. It should implement <see cref="IErrorOr"/> interface.</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
                                                                                              where TResponse : IErrorOr
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IValidator<TRequest>? validator;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="validator">The validator used in this behavior pipeline for the register command</param>
    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        this.validator = validator;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Pipeline handler. Perform any additional behavior and await the <paramref name="next"/> delegate as necessary
    /// </summary>
    /// <param name="request">Incoming request</param>
    /// <param name="next">Awaitable delegate for the next action in the pipeline. Eventually this delegate represents the handler.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // if there is no validator, just invoke the handler
        if (validator is null)
            return await next();
        // before the handler of the command is executed
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        // if there are no validation errors, invoke the command handler
        if (validationResult.IsValid)
            return await next();
        // after the command handler is executed
        var errors = validationResult.Errors.ConvertAll(validationFailure => Error.Validation(validationFailure.ErrorMessage));
        // the compiler doesn't know there is an implicit converter from a list of errors to the ErrorOr object, and unfortunately, there is no way around this but to use some magic
        // this is acceptable because we DO know that we will always have a list of errors of type ErrorOr (check the generic constraint at the top of the class!)
        return (dynamic)errors;
    }
    #endregion
}