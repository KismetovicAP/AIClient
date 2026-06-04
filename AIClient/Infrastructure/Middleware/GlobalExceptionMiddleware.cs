namespace AIClient.Infrastructure.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

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
