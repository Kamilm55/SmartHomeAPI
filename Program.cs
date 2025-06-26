using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Smart_Home_IoT_Device_Management_API.API.Middlewares;
using Smart_Home_IoT_Device_Management_API.Application.Mappers;
using Smart_Home_IoT_Device_Management_API.Application.Services;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Authentication;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// Service registrations
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SmartHomeContext>(options =>
    options.UseNpgsql(connectionString));

// JWT settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings.Key);


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

//

// Add services
builder.Services.AddControllers();
/*.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = 
        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});*/

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher,PasswordHasher>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDeviceService, DeviceService>();

builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddScoped<ISensorReadingService, SensorReadingService>();
builder.Services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();

builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddScoped<IDeviceCategoryRepository, DeviceCategoryRepository>();

builder.Services.AddSingleton<IMapper, CustomMapper>();


// Print the connection string 
builder.Logging.AddConsole();
//Console.Write("Connection string: " + connectionString);


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

    string rawPassword = "password";
    string hashedPassword = passwordHasher.Hash(rawPassword);

    Console.WriteLine($"Plain: {rawPassword}");
    Console.WriteLine($"Hashed: {hashedPassword}");

    var context = scope.ServiceProvider.GetRequiredService<SmartHomeContext>();
    SeedData.Initialize(context);
}

// Add global exception middleware near the top, before routing
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
