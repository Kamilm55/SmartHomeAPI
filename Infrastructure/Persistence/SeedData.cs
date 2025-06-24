using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Domain.Entities.Owned;
using Smart_Home_IoT_Device_Management_API.Domain.Entities.UserAndDevicePermission;
using Smart_Home_IoT_Device_Management_API.Domain.Enum;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;

public static class SeedData
{
 public static void Initialize(SmartHomeContext context)
{
    // Avoid seeding if data exists
    if (context.Users.Any() || context.DeviceCategories.Any() || context.Locations.Any() || context.Devices.Any())
        return;

    // 1. Users
    var users = new List<User>
    {
        new User {
            Id = Guid.NewGuid(),
            Username = "alice",
            Email = "alice@example.com",
            HashedPassword = "AbYzhro0DgHwA29fi1e9zW9VUT5Ilt7boAdhmSqB28MBZnK5mLsfnLgP0vk",
            FullName = "Alice Johnson",
            CreatedAt = DateTime.UtcNow.AddMonths(-6),
            LastLoginAt = DateTime.UtcNow.AddDays(-1)
        },
        new User {
            Id = Guid.NewGuid(),
            Username = "bob",
            Email = "bob@example.com",
            HashedPassword = "AXBHEgMIcoOBDzP/0MSV8wf2cRuTO3pp5qbSg5Pf0g/JMSVZieEcYS38YDvukTJOYw==",
            FullName = "Bob Smith",
            CreatedAt = DateTime.UtcNow.AddMonths(-4),
            LastLoginAt = DateTime.UtcNow.AddDays(-2)
        }
    };
    context.Users.AddRange(users);

    // 2. Locations
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
    context.Locations.AddRange(locations);

    // 3. Device Categories
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
    context.DeviceCategories.AddRange(categories);

    // 4. Devices
    var devices = new List<Device>
    {
        new Device
        {
            Id = Guid.NewGuid(),
            IsActive = true,
            InstalledAt = DateTime.UtcNow.AddMonths(-5),
            SerialNumber = "SN-THERMO-0001",
            PowerConsumption = 5.0f,
            MACAddress = "00:1A:C2:7B:00:47",
            LastCommunicationAt = DateTime.UtcNow.AddMinutes(-10),
            UsageCount = 1200,
            LastUsedAt = DateTime.UtcNow.AddMinutes(-5),
            DeviceCategoryId = categories[0].Id, // FK
            LocationId = locations[0].Id, // FK
            DeviceSetting = new DeviceSetting // Owned
            {
                Brightness = null,
                Volume = null,
                TemperatureThreshold = 22,
                AutoShutdown = false,
                MotionSensitivity = null,
                UpdateIntervalSeconds = 60
            },
            
            SensorData = new SensorData // One to Many olmalidi
            {
                Id = Guid.NewGuid(),
                Voltage = 230,
                Current = 0.05f,
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
        },
        new Device
        {
            Id = Guid.NewGuid(),
            IsActive = true,
            InstalledAt = DateTime.UtcNow.AddMonths(-3),
            SerialNumber = "SN-LIGHT-0023",
            PowerConsumption = 9.5f,
            MACAddress = "00:1B:C3:7B:00:50",
            LastCommunicationAt = DateTime.UtcNow.AddMinutes(-3),
            UsageCount = 800,
            LastUsedAt = DateTime.UtcNow.AddMinutes(-1),
            DeviceCategoryId = categories[1].Id,
            LocationId = locations[1].Id,
            DeviceSetting = new DeviceSetting
            {
                Brightness = 75,
                Volume = null,
                TemperatureThreshold = null,
                AutoShutdown = true,
                MotionSensitivity = null,
                UpdateIntervalSeconds = null
            },
            SensorData = new SensorData
            {
                Id = Guid.NewGuid(),
                Voltage = 230,
                Current = 0.04f,
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
        },
        new Device
        {
            Id = Guid.NewGuid(),
            IsActive = true,
            InstalledAt = DateTime.UtcNow.AddMonths(-1),
            SerialNumber = "SN-CAM-011",
            PowerConsumption = 4.0f,
            MACAddress = "00:1C:C4:7B:00:90",
            LastCommunicationAt = DateTime.UtcNow.AddMinutes(-15),
            UsageCount = 300,
            LastUsedAt = DateTime.UtcNow.AddMinutes(-10),
            DeviceCategoryId = categories[2].Id,
            LocationId = locations[2].Id,
            DeviceSetting = new DeviceSetting
            {
                Brightness = null,
                Volume = null,
                TemperatureThreshold = null,
                AutoShutdown = null,
                MotionSensitivity = 5,
                UpdateIntervalSeconds = 30
            },
            SensorData = new SensorData
            {
                Id = Guid.NewGuid(),
                Voltage = 3.7f,
                Current = 1.1f,
                PowerConsumptionWatts = 4,
                BatteryLevel = 85,
                SignalStrengthDb = -50,
                Temperature = 20,
                Humidity = 40,
                Pressure = 1010,
                LightLevel = 150,
                CO2Level = 400,
                MotionDetected = true,
                SoundLevel = 40,
                AirQualityIndex = 32,
                UptimeSeconds = 86400, // 1 day
                RecordedAt = DateTime.UtcNow
            }
        }
    };
    context.Devices.AddRange(devices);
    // also saves sensor data entities because of One To One relationship

    // 5. UserDevicePermissions
    var userDevicePermissions = new List<UserDevicePermission>
    {
        new UserDevicePermission
        {
            Id = Guid.NewGuid(),
            UserId = users[0].Id,
            DeviceId = devices[0].Id,
            Permission = PermissionLevel.ReadWrite
        },
        new UserDevicePermission
        {
            Id = Guid.NewGuid(),
            UserId = users[0].Id,
            DeviceId = devices[1].Id,
            Permission = PermissionLevel.Read
        },
        new UserDevicePermission
        {
            Id = Guid.NewGuid(),
            UserId = users[1].Id,
            DeviceId = devices[2].Id,
            Permission = PermissionLevel.ReadWrite
        }
    };
    context.UserDevicePermissions.AddRange(userDevicePermissions);

    // Save all seeded data
    context.SaveChanges();
}
   
}