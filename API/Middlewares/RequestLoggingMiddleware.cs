namespace Smart_Home_IoT_Device_Management_API.API.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        _logger.LogInformation($"HTTP {context.Request.Method} - {context.Request.Path}");
        await _next(context);
        _logger.LogInformation($"HTTP {context.Response.StatusCode}");
    }
}
