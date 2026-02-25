
using Microsoft.AspNetCore.Mvc;
using Oasis.Api.Logging;

namespace Oasis.Api.Middleware;

/// <summary>
/// 全局异常处理中间件
/// </summary>
public class ExceptionHandler(ILogger<ExceptionHandler> logger, IWebHostEnvironment env) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.UnhandledException(ex, context.TraceIdentifier);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "服务器内部错误",
                Status = StatusCodes.Status500InternalServerError,
                Detail = env.IsDevelopment() ? ex.ToString() : "发生了未知错误，请联系管理员。",
                Extensions =
                {
                    { "traceId", context.TraceIdentifier }
                }
            };

            await context.Response.WriteAsJsonAsync(problemDetails).ConfigureAwait(false);
        }
    }
}
