using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Oasis.Api.Filters;

public sealed class FluentValidationActionFilter(IEnumerable<IValidator> validators) : IAsyncActionFilter
{
    private readonly IValidator[] _validators = [.. validators];

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var errors = new Dictionary<string, string[]>();

        foreach (var arg in context.ActionArguments.Values)
        {
            if (arg is null) continue;

            var argType = arg.GetType();
            var matchedValidators = _validators.Where(v => v.CanValidateInstancesOfType(argType));

            //并行执行验证器
            var results = await Task.WhenAll(matchedValidators.Select(v => v.ValidateAsync(
                   new ValidationContext<object>(arg), context.HttpContext.RequestAborted)));

            foreach (var result in results)
            {
                if (result.IsValid) continue;

                foreach (var group in result.Errors.GroupBy(e => e.PropertyName))
                {
                    errors[group.Key] = [.. group.Select(e => e.ErrorMessage).Distinct()];
                }
            }
        }

        if (errors.Count > 0)
        {
            context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors));
            return;
        }

        await next();
    }
}