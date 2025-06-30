using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smart_Home_IoT_Device_Management_API.Extensions;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// --- Database ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SmartHomeContext>(options => options.UseNpgsql(connectionString));

// --- Service Registrations ---
builder.Services.AddApplicationServices();

// --- Identity ---
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<SmartHomeContext>()
    .AddDefaultTokenProviders();

// --- JWT, Auth, Swagger---
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorization();
builder.Services.AddSwaggerWithJwt();

// --- Middleware, Logging ---
builder.Logging.AddConsole();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// --- Seed ---
using (var scope = app.Services.CreateScope()) // The using ensures the scope and all its services are disposed properly after use.
{
    var context = scope.ServiceProvider.GetRequiredService<SmartHomeContext>();// Retrieves an instance of DbContext (SmartHomeContext) from the DI container.
    var seeder = scope.ServiceProvider.GetRequiredService<ISeedData>();
    await seeder.InitializeAsync(context);
}

// --- Middleware pipeline ---
app.UseGlobalMiddlewares();

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