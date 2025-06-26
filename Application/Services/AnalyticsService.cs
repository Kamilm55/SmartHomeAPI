using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Analytics;
using Smart_Home_IoT_Device_Management_API.Common.Utils;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly ISensorReadingRepository _sensorReadingRepository;

    public AnalyticsService(IDeviceRepository deviceRepository, ISensorReadingRepository sensorReadingRepository)
    {
        _deviceRepository = deviceRepository;
        _sensorReadingRepository = sensorReadingRepository;
    }

    public async Task<DeviceStatusSummaryResponse> GetDeviceStatusSummaryAsync()
    {
        List<Device?> allDevices = await _deviceRepository.GetAllWithCategoryAndLocationAsync();

        int onlineCount = allDevices.Count(d => d.IsActive);
        int offlineCount = allDevices.Count(d => !d.IsActive);

        var deviceDtos = allDevices.Select(d => new DevicesResponseInSummary()
        {
            DeviceId = d.Id,
            DeviceCategoryName = d.DeviceCategory.Name,
            LocationName =  d.Location.Name,
            IsActive = d.IsActive
        }).ToList();

        return new DeviceStatusSummaryResponse
        {
            OnlineCount = onlineCount,
            OfflineCount = offlineCount,
            DevicesResponseInSummary = deviceDtos
        };
    }

    public async Task<TotalEnergyUsageResponse> GetTotalEnergyUsageResponse(string deviceId)
    {
        Guid guid = GuidParser.Parse(deviceId,nameof(Device));
        List<SensorData> data = await _sensorReadingRepository.GetAllByDeviceIdAsync(guid);

        if (data == null) throw new ArgumentNullException();
        
        float totalWatts = data
            .Where(d => d.PowerConsumptionWatts.HasValue)
            .Sum(d => d.PowerConsumptionWatts.Value);

        float totalKWh = totalWatts / 1000f;
        
        return new TotalEnergyUsageResponse
        {
            TotalKWh = totalKWh
            //DeviceId = data.First().DeviceId,
            //DeviceCategoryName = data.First().Device.DeviceCategory.Name
        };
    }
}