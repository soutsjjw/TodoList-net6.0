using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ValidationException = TodoList.Application.Common.Exceptions.ValidationException;

namespace TodoList.Application.Common.Behaviors;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    // 注入所有自訂的Validators
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            // 如果有validator校驗失敗，拋出異常，這裡的異常是我們自訂的包裝類型
            if (failures.Any())
                throw new ValidationException(GetValidationErrorMessage(failures));
        }
        return await next();
    }

    // 格式化校驗失敗訊息
    private string GetValidationErrorMessage(IEnumerable<ValidationFailure> failures)
    {
        var failureDict = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

        return string.Join(";", failureDict.Select(kv => kv.Key + ": " + String.Join(' ', kv.Value.ToArray())));
    }
}

