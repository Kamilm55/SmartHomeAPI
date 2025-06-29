using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Domain.Entities.Owned;
using Smart_Home_IoT_Device_Management_API.Domain.Enum;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Security;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;

public class SeedData : ISeedData
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<User> _userManager;
    private ISeedData _seedDataImplementation;

    public SeedData(IPasswordHasher passwordHasher, RoleManager<IdentityRole<Guid>> roleManager, UserManager<User> userManager)
    {
        _passwordHasher = passwordHasher;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task InitializeAsync(SmartHomeContext context)
    {
        // 1. Seed Roles
        var rolesToSeed = new[] { "SuperAdmin", "Admin", "UserReadWrite", "UserReadOnly" };
        foreach (var roleName in rolesToSeed)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                if (!roleResult.Succeeded)
                {
                    Console.WriteLine($"Failed to create role {roleName}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }
            }
        }

        // 3. Avoid seeding domain data if any exists
        if (await context.DeviceCategories.AnyAsync() || await context.Locations.AnyAsync() || await context.Devices.AnyAsync())
        {
            Console.WriteLine("Initialize method in Seed data...");
            return;
        }

        // 4. Seed Locations
        var locations = new List<Location>
        {
            new Location
            {
                Id = Guid.NewGuid(),
                Name = "Living Room",
                Description = "Main family room",
                FloorNumber = 0,
                RoomId = 101
            },
            new Location
            {
                Id = Guid.NewGuid(),
                Name = "Kitchen",
                Description = "Cooking area",
                FloorNumber = 0,
                RoomId = 102
            },
            new Location
            {
                Id = Guid.NewGuid(),
                Name = "Garage",
                Description = "Vehicle storage",
                FloorNumber = -1,
                RoomId = 201
            }
        };
        await context.Locations.AddRangeAsync(locations);

        // 5. Seed Device Categories
        var categories = new List<DeviceCategory>
        {
            new DeviceCategory
            {
                Id = Guid.NewGuid(),
                Name = "Thermostat",
                Manufacturer = "Nest",
                FirmwareVersion = "5.1.2",
                PowerSource = PowerSource.Mains,
                RequiresInternet = true,
                Description = "Smart thermostat with remote control",
                CommunicationProtocol = "Wi-Fi",
                DeviceType = DeviceTypeGroup.ClimateControl
            },
            new DeviceCategory
            {
                Id = Guid.NewGuid(),
                Name = "Smart Light",
                Manufacturer = "Philips Hue",
                FirmwareVersion = "3.4.1",
                PowerSource = PowerSource.Mains,
                RequiresInternet = true,
                Description = "RGB LED smart bulb",
                CommunicationProtocol = "Zigbee",
                DeviceType = DeviceTypeGroup.Lighting
            },
            new DeviceCategory
            {
                Id = Guid.NewGuid(),
                Name = "Security Camera",
                Manufacturer = "Arlo",
                FirmwareVersion = "2.0.7",
                PowerSource = PowerSource.Battery,
                RequiresInternet = true,
                Description = "Wireless security camera",
                CommunicationProtocol = "Wi-Fi",
                DeviceType = DeviceTypeGroup.Security
            }
        };
        await context.DeviceCategories.AddRangeAsync(categories);

        // 6. Seed SensorData sets
        var sensorDataSet1 = new HashSet<SensorData>
        {
            new SensorData
            {
                Id = Guid.NewGuid(),
                //Voltage = 230,
                //Current = 0.05f,
                PowerConsumptionWatts = 5,
                BatteryLevel = null,
                SignalStrengthDb = -40,
                Temperature = 22.5f,
                Humidity = 45.0f,
                Pressure = 1013,
                LightLevel = 300,
                CO2Level = 400,
                MotionDetected = false,
                SoundLevel = 30,
                AirQualityIndex = 30,
                UptimeSeconds = 432000, // 5 days
                RecordedAt = DateTime.UtcNow
            }
        };

        var sensorDataSet2 = new HashSet<SensorData>
        {
            new SensorData
            {
                Id = Guid.NewGuid(),
                //Voltage = 230,
                //Current = 0.04f,
                PowerConsumptionWatts = 9.5f,
                BatteryLevel = null,
                SignalStrengthDb = -42,
                Temperature = 21,
                Humidity = 50,
                Pressure = 1012,
                LightLevel = 500,
                CO2Level = 390,
                MotionDetected = false,
                SoundLevel = 25,
                AirQualityIndex = 28,
                UptimeSeconds = 259200, // 3 days
                RecordedAt = DateTime.UtcNow
            }
        };

        // 7. Seed Devices
        var devices = new List<Device>
        {
            new Device
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                InstalledAt = DateTime.UtcNow.AddMonths(-5),
                SerialNumber = "SN-THERMO-0001",
                MACAddress = "00:1A:C2:7B:00:47",
                LastCommunicationAt = DateTime.UtcNow.AddMinutes(-10),
                DeviceCategoryId = categories[0].Id, // FK
                LocationId = locations[0].Id, // FK
                DeviceSetting = new DeviceSetting
                {
                    Id = Guid.NewGuid(),
                    Brightness = null,
                    Volume = null,
                    TemperatureThreshold = 22,
                    AutoShutdown = false,
                    MotionSensitivity = null,
                    UpdateIntervalSeconds = 60
                },
                SensorData = sensorDataSet1
            },
            new Device
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                InstalledAt = DateTime.UtcNow.AddMonths(-3),
                SerialNumber = "SN-LIGHT-0023",
                MACAddress = "00:1B:C3:7B:00:50",
                LastCommunicationAt = DateTime.UtcNow.AddMinutes(-3),
                
                DeviceCategoryId = categories[1].Id,
                LocationId = locations[1].Id,
                DeviceSetting = new DeviceSetting
                {
                    Id = Guid.NewGuid(),
                    Brightness = 75,
                    Volume = null,
                    TemperatureThreshold = null,
                    AutoShutdown = true,
                    MotionSensitivity = null,
                    UpdateIntervalSeconds = null
                },
                SensorData = sensorDataSet2
            },
            new Device
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                InstalledAt = DateTime.UtcNow.AddMonths(-1),
                SerialNumber = "SN-CAM-011",
                MACAddress = "00:1C:C4:7B:00:90",
                LastCommunicationAt = DateTime.UtcNow.AddMinutes(-15),
                DeviceCategoryId = categories[2].Id,
                LocationId = locations[2].Id,
                DeviceSetting = new DeviceSetting
                {
                    Id = Guid.NewGuid(),
                    Brightness = null,
                    Volume = null,
                    TemperatureThreshold = null,
                    AutoShutdown = null,
                    MotionSensitivity = 5,
                    UpdateIntervalSeconds = 30
                },
                SensorData = sensorDataSet2
            }
        };

        // 8. Seed Users (domain users, if you still need to, but avoid confusion with Identity users)
        // Get existing roles from DB (or store their IDs after seeding)
        var roles = context.Roles.ToDictionary(r => r.Name, r => r.Id);

// Create your users
        var domainUsers = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "alice",
                Email = "alice@example.com",
                PasswordHash = _passwordHasher.Hash("password"),
                FullName = "Alice Johnson",
                CreatedAt = DateTime.UtcNow.AddMonths(-6),
                SecurityStamp = Guid.NewGuid().ToString(),
                LastLoginAt = DateTime.UtcNow.AddDays(-1)
            },
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "bob",
                Email = "bob@example.com",
                PasswordHash = _passwordHasher.Hash("password"),
                FullName = "Bob Smith",
                CreatedAt = DateTime.UtcNow.AddMonths(-4),
                SecurityStamp = Guid.NewGuid().ToString(),
                LastLoginAt = DateTime.UtcNow.AddDays(-2)
            }
        };

// Manually assign roles by inserting into AspNetUserRoles
        var userRoles = new List<IdentityUserRole<Guid>>
        {
            new IdentityUserRole<Guid>
            {
                UserId = domainUsers[0].Id,
                RoleId = roles["SuperAdmin"] 
            },
            new IdentityUserRole<Guid>
            {
                UserId = domainUsers[1].Id,
                RoleId = roles["UserReadOnly"]
            }
        };

        // Add users and their roles to the context
        context.Users.AddRange(domainUsers);
        context.UserRoles.AddRange(userRoles); 

        // Assign shared devices
        foreach (var device in devices)
        {
            domainUsers[0].Devices.Add(device);
            device.Users.Add(domainUsers[0]);

            domainUsers[1].Devices.Add(device);
            device.Users.Add(domainUsers[1]);
        }

        await context.Devices.AddRangeAsync(devices);
        await context.Users.AddRangeAsync(domainUsers);

        // Save all seeded data
        await context.SaveChangesAsync();

        Console.WriteLine("Seed data completed.");
    }
}
