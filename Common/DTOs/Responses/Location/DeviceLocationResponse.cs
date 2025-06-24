using Smart_Home_IoT_Device_Management_API.Domain.Enum;

namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;

public class DeviceInLocationResponse
{
    public Guid Id { get; set; }
    public string SerialNumber { get; set; } = null!;
    public Guid DeviceCategoryid { get; set; }
    public string DeviceCategoryName { get; set; } = null!;
    public DeviceTypeGroup? DeviceType { get; set; }
    public bool IsActive { get; set; }
    public DateTime InstalledAt { get; set; }
    public float? PowerConsumption { get; set; }
}