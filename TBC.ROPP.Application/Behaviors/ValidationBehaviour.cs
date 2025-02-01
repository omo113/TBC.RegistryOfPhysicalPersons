using FluentValidation;
using MediatR;
using TBC.ROPP.Shared.ApplicationInfrastructure;

namespace TBC.ROPP.Application.Behaviors;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                validators.Select(v =>
                    v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
            {
                var validationErrors = new ApplicationError(
                    failures.Select(x => new Error(x.ErrorCode, x.ErrorMessage)).ToList()
                );

                var responseType = typeof(TResponse);

                var response = (TResponse)Activator.CreateInstance(
                    responseType,
                    validationErrors
                )!;

                return response;
            }
        }
        var resp = await next();
        return resp;
    }
}