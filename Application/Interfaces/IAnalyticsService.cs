using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Analytics;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public interface IAnalyticsService
{
    Task<DeviceStatusSummaryResponse> GetDeviceStatusSummaryAsync();
    Task<TotalEnergyUsageResponse> GetTotalEnergyUsageResponse(string deviceId);
}