namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Analytics;

public class DevicesResponseInSummary
{
    public Guid DeviceId { get; set; }
    public string DeviceCategoryName { get; set; } = null!;
    public bool IsActive { get; set; }
    public string LocationName { get; set; }
}