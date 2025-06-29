using Smart_Home_IoT_Device_Management_API.Domain.Enum;

namespace Smart_Home_IoT_Device_Management_API.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole(nameof(Role.SuperAdmin)));
            options.AddPolicy("AdminOnly", policy => policy.RequireRole(nameof(Role.Admin)));
            options.AddPolicy("UserReadWriteOnly", policy => policy.RequireRole(nameof(Role.UserReadWrite)));
            options.AddPolicy("UserReadOnlyOnly", policy => policy.RequireRole(nameof(Role.UserReadOnly)));
        });

        return services;
    }
}