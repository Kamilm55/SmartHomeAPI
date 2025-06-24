namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Analytics;

public class TotalEnergyUsageResponse
{
    public float TotalKWh { get; set; }
    public string TimeRange { get; set; } = null!;
}