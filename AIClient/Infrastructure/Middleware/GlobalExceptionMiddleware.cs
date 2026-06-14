namespace AIClient.Infrastructure.Middleware;

/// <summary>
/// ASP.NET Core middleware that catches unhandled exceptions, logs them,
/// and writes a safe plain-text error message to the HTTP response.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="GlobalExceptionMiddleware"/>.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">Logger for unhandled exception telemetry.</param>
    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the next middleware and intercepts any unhandled exception.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred during request processing");
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Writes a type-mapped, user-safe error message and sets HTTP 500 on the response.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="exception">The unhandled exception.</param>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "text/plain";

        var message = exception switch
        {
            ArgumentException => "Invalid request parameters.",
            UnauthorizedAccessException => "Unauthorized access.",
            InvalidOperationException => "The operation is invalid in the current state.",
            HttpRequestException => "An error occurred while communicating with external services.",
            _ => "An internal server error occurred."
        };

        return context.Response.WriteAsync(message);
    }
}
