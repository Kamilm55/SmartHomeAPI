namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Analytics;

public class DeviceStatusSummaryResponse
{
    public int OnlineCount { get; set; }
    public int OfflineCount { get; set; }
    
    public List<DevicesResponseInSummary> DevicesResponseInSummary { get; set; }
       
}