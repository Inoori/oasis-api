namespace Oasis.Api.Logging;

/// <summary>
/// 源日志生成器方式记录日志
/// </summary>
public static partial class Log
{

    /// <summary>
    /// 未处理的异常
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="traceId"></param>
    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Unhandled exception, TraceId: {TraceId}")]
    public static partial void UnhandledException(this ILogger logger, Exception exception, string traceId);

}
