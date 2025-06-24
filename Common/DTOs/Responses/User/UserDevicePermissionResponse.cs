using Smart_Home_IoT_Device_Management_API.Domain.Enum;

namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;

public class UserDevicePermissionResponse
{
    // Device
    public Guid DeviceId { get; set; }
    public string SerialNumber { get; set; } = null!;
    // Device Category
    public string DeviceCategory { get; set; } = null!;
    public DeviceTypeGroup? DeviceTypeGroup { get; set; }
    // Location
    public string LocationName { get; set; } = null!;
    
    public PermissionLevel Permission { get; set; }
}