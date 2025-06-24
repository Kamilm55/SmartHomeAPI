using Smart_Home_IoT_Device_Management_API.Domain.Enum;

namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Device;

public class DeviceResponse
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime InstalledAt { get; set; }
    public string SerialNumber { get; set; } = null!;
    public float? PowerConsumption { get; set; }
    public string? MACAddress { get; set; }
    public DateTime? LastCommunicationAt { get; set; }
    public int? UsageCount { get; set; }
    public DateTime? LastUsedAt { get; set; }

    // Device Setting 
    public int? Brightness { get; set; }
    public int? Volume { get; set; }
    public int? TemperatureThreshold { get; set; }
    public bool? AutoShutdown { get; set; }
    public int? MotionSensitivity { get; set; }
    public int? UpdateIntervalSeconds { get; set; }

    // Device Category
    public Guid DeviceCategoryId { get; set; }
    public string DeviceCategoryName { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public string FirmwareVersion { get; set; } = null!;
    public PowerSource PowerSource { get; set; }
    public bool RequiresInternet { get; set; }
    public string? CommunicationProtocol { get; set; }
    public DeviceTypeGroup? DeviceType { get; set; }

    // Location
    public Guid LocationId { get; set; }
    public string LocationName { get; set; } = null!;
    public int? FloorNumber { get; set; }

    // Sensor Data
    /*public float? Voltage { get; set; }
    public float? Current { get; set; }
    public float? PowerConsumptionWatts { get; set; }
    public int? BatteryLevel { get; set; }
    public float? SignalStrengthDb { get; set; }
    public float? Temperature { get; set; }
    public float? Humidity { get; set; }
    public float? Pressure { get; set; }
    public float? LightLevel { get; set; }
    public float? CO2Level { get; set; }
    public bool? MotionDetected { get; set; }
    public float? SoundLevel { get; set; }
    public int? AirQualityIndex { get; set; }
    public long? UptimeSeconds { get; set; }
    public DateTime? SensorRecordedAt { get; set; }*/
}