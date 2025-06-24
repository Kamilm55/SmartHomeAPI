using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Analytics.Enum;

namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Analytics;

public class DeviceHealthStatusResponse
{
    public Guid DeviceId { get; set; }
    public string SerialNumber { get; set; } = null!;
    public int? BatteryLevel { get; set; }
    public float? SignalStrengthDb { get; set; }
    public DateTime? LastCommunicationAt { get; set; }
    public DeviceHealthStatus Status { get; set; } = DeviceHealthStatus.Healthy; 
}