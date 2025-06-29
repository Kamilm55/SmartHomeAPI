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
        Guid guid = GuidParser.Parse(deviceId,nameof(Device));
        List<SensorData> data = await _sensorReadingRepository.GetAllByDeviceIdAsync(guid);
        
        
       await _helperService.IsThisDeviceBelongsToCurrentUser(guid);
        
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
    
    public async Task<DeviceStatusSummaryResponse> GetDeviceStatusSummaryAsync()
    {
        var currentUser = await _helperService.getCurrentUserFromToken();
        List<Device> allDevices = await _deviceRepository.GetAllByUserIdWithCategoryAndLocationAsync(currentUser.Id);
        
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

    public async Task<LocationUsageStatsResponse> GetUsageByLocationAsync(string locationId)
    {
        var currentUser = await _helperService.getCurrentUserFromToken();

        Guid guid = GuidParser.Parse(locationId, nameof(Location));

        var location = await _locationRepository.GetByIdAsync(guid)
            ?? throw new NotFoundException(nameof(Location),guid);
        var sensorDataList = await _sensorReadingRepository.GetAllByDevicesAndByLocationAsync(currentUser.Devices,guid);

        float result = sensorDataList.
            Where(d => d.PowerConsumptionWatts.HasValue)
            .Sum(d => d.PowerConsumptionWatts.Value);

        return new LocationUsageStatsResponse
        {
            LocationName = location.Name,
            EnergyUsedKWh = result
           // DeviceCount 
        };
    }

    public async Task<DeviceHealthStatusResponse> GetDeviceHealthStatusAsync(string deviceId)
    {
        Guid guid = GuidParser.Parse(deviceId, nameof(Device));
        var device = await _deviceRepository.GetByIdAsync(guid);
        var data = await _sensorReadingRepository.GetLatestByDeviceIdAsync(guid); // latest reading per device

        DeviceHealthStatusResponse response = new DeviceHealthStatusResponse
        {
            DeviceId = deviceId,
            SerialNumber = device?.SerialNumber ?? "",
            BatteryLevel = data?.BatteryLevel,
            SignalStrengthDb = data?.SignalStrengthDb,
            LastCommunicationAt = device?.LastCommunicationAt,
            Status = GetDeviceHealthStatus(data?.BatteryLevel, data?.SignalStrengthDb, device?.LastCommunicationAt)
        };

        return response;
    }

    private DeviceHealthStatus GetDeviceHealthStatus(int? batteryLevel, float? signalStrengthDb, DateTime? lastCommunicationAt)
        {
            if (lastCommunicationAt == null || lastCommunicationAt < DateTime.UtcNow.AddHours(-12))
            {
                return DeviceHealthStatus.Critical;
            }

            if (batteryLevel.HasValue && batteryLevel < 20)
            {
                return DeviceHealthStatus.Warning;
            }

            if (signalStrengthDb.HasValue && signalStrengthDb < -100)
            {
                return DeviceHealthStatus.Warning;
            }

            return DeviceHealthStatus.Healthy;
        }

    }
