using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Analytics;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Analytics.Enum;
using Smart_Home_IoT_Device_Management_API.Common.Exceptions;
using Smart_Home_IoT_Device_Management_API.Common.Utils;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly ISensorReadingRepository _sensorReadingRepository;
    private readonly IHelperService _helperService;
    private readonly ILocationRepository _locationRepository;
    
    public AnalyticsService(IDeviceRepository deviceRepository, ISensorReadingRepository sensorReadingRepository, IHelperService helperService, ILocationRepository locationRepository)
    {
        _deviceRepository = deviceRepository;
        _sensorReadingRepository = sensorReadingRepository;
        _helperService = helperService;
        _locationRepository = locationRepository;
    }
      public async Task<TotalEnergyUsageResponse> GetTotalEnergyUsageResponse(string deviceId)
    {
        var guid = await ValidateAccessAndGetDeviceGuid(deviceId);
        var data = await _sensorReadingRepository.GetAllByDeviceIdAsync(guid);

        float totalKWh = CalculateTotalKWh(data);

        return new TotalEnergyUsageResponse
        {
            TotalKWh = totalKWh
        };
    }

    public async Task<DeviceStatusSummaryResponse> GetDeviceStatusSummaryAsync()
    {
        var currentUser = await _helperService.getCurrentUserFromToken();
        var allDevices = await _deviceRepository.GetAllByUserIdWithCategoryAndLocationAsync(currentUser.Id);

        return new DeviceStatusSummaryResponse
        {
            OnlineCount = allDevices.Count(d => d.IsActive),
            OfflineCount = allDevices.Count(d => !d.IsActive),
            DevicesResponseInSummary = allDevices.Select(d => new DevicesResponseInSummary
            {
                DeviceId = d.Id,
                DeviceCategoryName = d.DeviceCategory.Name,
                LocationName = d.Location.Name,
                IsActive = d.IsActive
            }).ToList()
        };
    }

    public async Task<LocationUsageStatsResponse> GetUsageByLocationAsync(string locationId)
    {
        var locationGuid = GuidParser.Parse(locationId, nameof(Location));
        var currentUser = await _helperService.getCurrentUserFromToken();

        var location = await _locationRepository.GetByIdAsync(locationGuid)
            ?? throw new NotFoundException(nameof(Location), locationGuid);

        var sensorDataList = await _sensorReadingRepository
            .GetAllByDevicesAndByLocationAsync(currentUser.Devices, locationGuid);

        float totalKWh = CalculateTotalKWh(sensorDataList);

        return new LocationUsageStatsResponse
        {
            LocationName = location.Name,
            EnergyUsedKWh = totalKWh
        };
    }

    public async Task<DeviceHealthStatusResponse> GetDeviceHealthStatusAsync(string deviceId)
    {
        var guid = GuidParser.Parse(deviceId, nameof(Device));
        var device = await _deviceRepository.GetByIdAsync(guid);
        var data = await _sensorReadingRepository.GetLatestByDeviceIdAsync(guid);

        return new DeviceHealthStatusResponse
        {
            DeviceId = deviceId,
            SerialNumber = device?.SerialNumber ?? "",
            BatteryLevel = data?.BatteryLevel,
            SignalStrengthDb = data?.SignalStrengthDb,
            LastCommunicationAt = device?.LastCommunicationAt,
            Status = GetDeviceHealthStatus(data?.BatteryLevel, data?.SignalStrengthDb, device?.LastCommunicationAt)
        };
    }

    // Helpers

    private async Task<Guid> ValidateAccessAndGetDeviceGuid(string deviceId)
    {
        var guid = GuidParser.Parse(deviceId, nameof(Device));
        await _helperService.IsThisDeviceBelongsToCurrentUser(guid);
        return guid;
    }

    private float CalculateTotalKWh(IEnumerable<SensorData> data)
    {
        float totalWatts = data
            .Where(d => d.PowerConsumptionWatts.HasValue)
            .Sum(d => d.PowerConsumptionWatts.Value);

        return totalWatts / 1000f;
    }

    private DeviceHealthStatus GetDeviceHealthStatus(int? batteryLevel, float? signalStrengthDb, DateTime? lastCommunicationAt)
    {
        if (lastCommunicationAt == null || lastCommunicationAt < DateTime.UtcNow.AddHours(-12))
            return DeviceHealthStatus.Critical;

        if (batteryLevel.HasValue && batteryLevel < 20)
            return DeviceHealthStatus.Warning;

        if (signalStrengthDb.HasValue && signalStrengthDb < -100)
            return DeviceHealthStatus.Warning;

        return DeviceHealthStatus.Healthy;
    }

    }
