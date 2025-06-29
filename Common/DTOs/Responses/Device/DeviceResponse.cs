using Smart_Home_IoT_Device_Management_API.Domain.Enum;

namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Device;

public class DeviceResponse
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime InstalledAt { get; set; }
    public string SerialNumber { get; set; } = null!;
    public string? MACAddress { get; set; }
    public DateTime? LastCommunicationAt { get; set; }

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

}