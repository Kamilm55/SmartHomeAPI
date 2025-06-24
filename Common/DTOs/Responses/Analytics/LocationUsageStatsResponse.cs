namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Analytics;

public class LocationUsageStatsResponse
{
    public string LocationName { get; set; } = null!;
    public float EnergyUsedKWh { get; set; }
    public int DeviceCount { get; set; }
}