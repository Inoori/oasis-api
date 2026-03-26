

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
                Detail = string.Join(Environment.NewLine, result.Errors.Select(e => e.Message))
            };
            return new BadRequestObjectResult(problemDetails);
        }

        public IActionResult ToActionResult(int statusCode)
        {
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = "One or more errors occurred.",
                Detail = string.Join(Environment.NewLine, result.Errors.Select(e => e.Message))
            };
            return new ObjectResult(problemDetails) { StatusCode = statusCode };
        }
    }

    extension<T>(Result<T> result)
    {
        public IActionResult ToActionResult()
        {
            var problemDetails = new ProblemDetails
            {
                Status = 400,
                Title = "One or more errors occurred.",
                Detail = string.Join(Environment.NewLine, result.Errors.Select(e => e.Message))
            };
            return new BadRequestObjectResult(problemDetails);
        }
        public IActionResult ToActionResult(int statusCode)
        {
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = "One or more errors occurred.",
                Detail = string.Join(Environment.NewLine, result.Errors.Select(e => e.Message))
            };
            return new ObjectResult(problemDetails) { StatusCode = statusCode };
        }
    }
}