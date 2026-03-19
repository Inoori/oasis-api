using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Oasis.Api.Middleware;


/// <summary>
/// 全局异常处理器，捕获未处理的异常并返回标准化的 ProblemDetails 响应
/// </summary>
/// <param name="logger"></param>
/// <param name="problemDetailsService"></param>
/// <param name="hostEnvironment"></param>
public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService,
    IHostEnvironment hostEnvironment) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;

    private readonly IHostEnvironment _hostEnvironment = hostEnvironment;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.UnhandledException(exception, httpContext.TraceIdentifier);


        var statusCode = exception switch
        {
            BadHttpRequestException bad => bad.StatusCode,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        string title = statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad Request",
            StatusCodes.Status401Unauthorized => "Unauthorized",
            StatusCodes.Status404NotFound => "Not Found",
            _ => "Server Error"
        };

        string? detail = _hostEnvironment.IsDevelopment()
            ? exception.ToString()
            : "An unexpected error occurred. Please try again later.";


        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };

        httpContext.Response.StatusCode = statusCode;

        // 使用 ProblemDetailsService 自动生成标准响应
        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetails
        });
    }
}