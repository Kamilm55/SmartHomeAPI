using Smart_Home_IoT_Device_Management_API.API.Middlewares;

namespace Smart_Home_IoT_Device_Management_API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        return app;
    }
}