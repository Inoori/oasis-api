

using Microsoft.AspNetCore.Mvc;

namespace FluentResults;

public static class ResultProblemDetailsExtensions
{
    extension(Result result)
    {
        public IActionResult ToActionResult()
        {
            var problemDetails = new ProblemDetails
            {
                Status = 400,
                Title = "One or more errors occurred.",
                Detail = string.Join("; ", result.Errors.Select(e => e.Message))
            };
            return new BadRequestObjectResult(problemDetails);
        }
    }
}