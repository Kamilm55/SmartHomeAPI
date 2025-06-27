namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;

public class DeviceDTOInUserResponse
{
    // Device
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime InstalledAt { get; set; }
    public string SerialNumber { get; set; } = null!;
    
    // Device Category
    public string DeviceCategoryName { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
}