using Smart_Home_IoT_Device_Management_API.Application.Mappers;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Device;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Device;
using Smart_Home_IoT_Device_Management_API.Common.Exceptions;
using Smart_Home_IoT_Device_Management_API.Common.Utils;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Domain.Entities.Owned;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IDeviceCategoryRepository _deviceCategoryRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeviceService(
        IDeviceRepository deviceRepository,
        IDeviceCategoryRepository deviceCategoryRepository,
        ILocationRepository locationRepository,
        IUnitOfWork unitOfWork)
    {
        _deviceRepository = deviceRepository;
        _deviceCategoryRepository = deviceCategoryRepository;
        _locationRepository = locationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<DeviceResponse>> GetAllDevicesAsync()
    {
        var devices = await _deviceRepository.GetAllWithCategoryAndLocationAsync();
        return devices.Select(DeviceMapper.ToDeviceResponse).ToList();
    }

    public async Task<DeviceResponse> GetDeviceByIdAsync(string id)
    {
        Guid guid = GuidParser.Parse(id,nameof(Device));

        var device = await _deviceRepository.GetByIdWithCategoryAndLocationAsync(guid)
                     ?? throw new NotFoundException("Device", guid);

        return DeviceMapper.ToDeviceResponse(device);
    }

    public async Task<DeviceResponse> CreateDeviceAsync(DeviceCreateRequest request)
    {
        var category = await _deviceCategoryRepository.GetByIdAsync(request.DeviceCategoryId)
                       ?? throw new NotFoundException("DeviceCategory", request.DeviceCategoryId);

        var location = await _locationRepository.GetByIdAsync(request.LocationId)
                       ?? throw new NotFoundException("Location", request.LocationId);

        var device = new Domain.Entities.Device
        {
            Id = Guid.NewGuid(),
            SerialNumber = request.SerialNumber,
            InstalledAt = DateTime.UtcNow,
            PowerConsumption = request.PowerConsumption,
            MACAddress = request.MACAddress,
            DeviceCategoryId = category.Id,
            LocationId = location.Id,
            DeviceSetting = new DeviceSetting
            {
                Brightness = request.Brightness,
                Volume = request.Volume,
                TemperatureThreshold = request.TemperatureThreshold,
                AutoShutdown = request.AutoShutdown,
                MotionSensitivity = request.MotionSensitivity,
                UpdateIntervalSeconds = request.UpdateIntervalSeconds
            }
        };

        await _deviceRepository.AddAsync(device);
        Device createdDevice = await _deviceRepository.SaveDeviceAndReturnLatest(device);

        return DeviceMapper.ToDeviceResponse(createdDevice);
    }

    public async Task<DeviceResponse> UpdateDeviceAsync(string id, DeviceUpdateRequest request)
    {
        Guid guid = GuidParser.Parse(id, nameof(Device));

        var device = await _deviceRepository.GetByIdWithCategoryAndLocationAsync(guid)
                     ?? throw new NotFoundException("Device", guid);

        device.SerialNumber = request.SerialNumber ?? device.SerialNumber;
        device.PowerConsumption = request.PowerConsumption ?? device.PowerConsumption;
        device.MACAddress = request.MACAddress ?? device.MACAddress;
        device.IsActive = request.IsActive ?? device.IsActive;

        if (request.DeviceCategoryId is not null)
        {
          await _deviceCategoryRepository.ExistsByIdAsync(request.DeviceCategoryId);
        }
        if (request.LocationId is not null)
        {
          await _locationRepository.ExistsByIdAsync(request.LocationId);
        }

        device.DeviceCategoryId = request.DeviceCategoryId ?? device.DeviceCategoryId;
        device.LocationId = request.LocationId ?? device.LocationId;

        if (device.DeviceSetting != null)
        {
            device.DeviceSetting.Brightness = request.Brightness ?? device.DeviceSetting.Brightness;
            device.DeviceSetting.Volume = request.Volume ?? device.DeviceSetting.Volume;
            device.DeviceSetting.TemperatureThreshold = request.TemperatureThreshold ?? device.DeviceSetting.TemperatureThreshold;
            device.DeviceSetting.AutoShutdown = request.AutoShutdown ?? device.DeviceSetting.AutoShutdown;
            device.DeviceSetting.MotionSensitivity = request.MotionSensitivity ?? device.DeviceSetting.MotionSensitivity;
            device.DeviceSetting.UpdateIntervalSeconds = request.UpdateIntervalSeconds ?? device.DeviceSetting.UpdateIntervalSeconds;
        }
        
        Device savedDevice = await _deviceRepository.SaveDeviceAndReturnLatest(device);
        return DeviceMapper.ToDeviceResponse(savedDevice);
        //return DeviceMapper.ToDeviceResponse(await _deviceRepository.GetByIdWithCategoryAndLocationAsync(device.Id););
    }

    public async Task DeleteDeviceAsync(string id)
    {
        Guid guid = GuidParser.Parse(id,nameof(Device));

        var device = await _deviceRepository.GetByIdWithCategoryAndLocationAsync(guid)
                     ?? throw new NotFoundException("Device", guid);

        _deviceRepository.Delete(device);
        await _deviceRepository.SaveDeviceAndReturnLatest(device);
    }
    
    
}