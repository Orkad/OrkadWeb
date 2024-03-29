﻿using System.Collections.Generic;
using System.Linq;

namespace OrkadWeb.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IBaseRequest
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }
        var context = new ValidationContext<TRequest>(request);
        var errors = _validators
            .Select(async v => await v.ValidateAsync(context, cancellationToken)).Select(t => t.Result)
            .Where(v => !v.IsValid).SelectMany(v => v.Errors)
            .ToList();
        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
        return await next();
    }
}