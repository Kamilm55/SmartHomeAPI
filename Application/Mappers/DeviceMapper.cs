using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Device;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Application.Mappers;

public static class DeviceMapper
{
    public static DeviceResponse ToDeviceResponse(Device device)
    {
        return new DeviceResponse
        {
            Id = device.Id,
            IsActive = device.IsActive,
            InstalledAt = device.InstalledAt,
            SerialNumber = device.SerialNumber,
            PowerConsumption = device.PowerConsumption,
            MACAddress = device.MACAddress,
            LastCommunicationAt = device.LastCommunicationAt,
            UsageCount = device.UsageCount,
            LastUsedAt = device.LastUsedAt,

            // DeviceSetting 
            Brightness = device.DeviceSetting?.Brightness,
            Volume = device.DeviceSetting?.Volume,
            TemperatureThreshold = device.DeviceSetting?.TemperatureThreshold,
            AutoShutdown = device.DeviceSetting?.AutoShutdown,
            MotionSensitivity = device.DeviceSetting?.MotionSensitivity,
            UpdateIntervalSeconds = device.DeviceSetting?.UpdateIntervalSeconds,

            // Category
            DeviceCategoryId = device.DeviceCategory.Id,
            DeviceCategoryName = device.DeviceCategory.Name,
            Manufacturer = device.DeviceCategory.Manufacturer,
            FirmwareVersion = device.DeviceCategory.FirmwareVersion,
            PowerSource = device.DeviceCategory.PowerSource,
            RequiresInternet = device.DeviceCategory.RequiresInternet,
            CommunicationProtocol = device.DeviceCategory.CommunicationProtocol,
            DeviceType = device.DeviceCategory.DeviceType,

            // Location
            LocationId = device.Location.Id,
            LocationName = device.Location.Name,
            FloorNumber = device.Location.FloorNumber
        };
    }
}