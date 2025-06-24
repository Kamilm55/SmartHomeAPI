namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Device;

public class DeviceUpdateRequest
{
    public bool? IsActive { get; set; }
    public string? SerialNumber { get; set; }
    public float? PowerConsumption { get; set; }
    public string? MACAddress { get; set; }
    public DateTime? LastCommunicationAt { get; set; }
    public int? UsageCount { get; set; }
    public DateTime? LastUsedAt { get; set; }

    // Settings
    public int? Brightness { get; set; }
    public int? Volume { get; set; }
    public int? TemperatureThreshold { get; set; }
    public bool? AutoShutdown { get; set; }
    public int? MotionSensitivity { get; set; }
    public int? UpdateIntervalSeconds { get; set; }

    // Foreign Keys (optional update)
    public Guid? DeviceCategoryId { get; set; }// // to change category of device
    public Guid? LocationId { get; set; } // to change location of device
}