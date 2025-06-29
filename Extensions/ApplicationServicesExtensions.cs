using Microsoft.AspNetCore.Identity;
using Smart_Home_IoT_Device_Management_API.Application.Mappers;
using Smart_Home_IoT_Device_Management_API.Application.Services;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Authentication;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Security;

namespace Smart_Home_IoT_Device_Management_API.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IHelperService, HelperService>();

        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IDeviceService, DeviceService>();

        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<ILocationService, LocationService>();

        services.AddScoped<ISensorReadingService, SensorReadingService>();
        services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();

        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<IDeviceCategoryRepository, DeviceCategoryRepository>();

        services.AddScoped<ISeedData, SeedData>();
        services.AddSingleton<IMapper, CustomMapper>();

        return services;
    }
}