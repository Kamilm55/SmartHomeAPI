namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Device;

public class DeviceCreateRequest
{
    public bool IsActive { get; set; } = true;
    public DateTime InstalledAt { get; set; }
    public string SerialNumber { get; set; } = null!;
    public float? PowerConsumption { get; set; }
    public string? MACAddress { get; set; }
    
    // Settings
    public int? Brightness { get; set; }
    public int? Volume { get; set; }
    public int? TemperatureThreshold { get; set; }
    public bool? AutoShutdown { get; set; }
    public int? MotionSensitivity { get; set; }
    public int? UpdateIntervalSeconds { get; set; }

    // Foreign Keys
    public Guid DeviceCategoryId { get; set; }
    public Guid LocationId { get; set; }
}