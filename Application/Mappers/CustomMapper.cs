using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Device;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Location;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.SensorData;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Domain.Entities.Owned;

namespace Smart_Home_IoT_Device_Management_API.Application.Mappers;

public class CustomMapper : IMapper
{
    public LocationResponse ToLocationResponse(Location location)
    {
        return new LocationResponse
        {
            Id = location.Id,
            Name = location.Name,
            Description = location.Description,
            FloorNumber = location.FloorNumber,
            RoomId = location.RoomId
        };
    }  
    
    public DeviceInLocationResponse ToDeviceInLocationResponse(Device device)
    {
        return new DeviceInLocationResponse
        {
            Id = device.Id,
            SerialNumber = device.SerialNumber,
            IsActive = device.IsActive,
            InstalledAt = device.InstalledAt,
            DeviceType = device.DeviceCategory.DeviceType,
            DeviceCategoryid = device.DeviceCategoryId,
            DeviceCategoryName = device.DeviceCategory.Name
            //PowerConsumption = device.PowerConsumption,
        };
    }
    
    public SensorData ToSensorData(SensorDataRequest request , Guid deviceId)
    {
        return new SensorData
        {
            PowerConsumptionWatts = request.PowerConsumptionWatts,
            BatteryLevel = request.BatteryLevel,
            SignalStrengthDb = request.SignalStrengthDb,
            Temperature = request.Temperature,
            Humidity = request.Humidity,
            Pressure = request.Pressure,
            LightLevel = request.LightLevel,
            CO2Level = request.CO2Level,
            MotionDetected = request.MotionDetected,
            SoundLevel = request.SoundLevel,
            AirQualityIndex = request.AirQualityIndex,
            UptimeSeconds = request.UptimeSeconds,
            DeviceId = deviceId
        };
    }

    public SensorDataResponse ToSensorDataResponse(SensorData sensorData)
    {
       return new SensorDataResponse
        {
            Id = sensorData.Id,
            PowerConsumptionWatts = sensorData.PowerConsumptionWatts,
            BatteryLevel = sensorData.BatteryLevel,
            SignalStrengthDb = sensorData.SignalStrengthDb,
            Temperature = sensorData.Temperature,
            Humidity = sensorData.Humidity,
            Pressure = sensorData.Pressure,
            LightLevel = sensorData.LightLevel,
            CO2Level = sensorData.CO2Level,
            MotionDetected = sensorData.MotionDetected,
            SoundLevel = sensorData.SoundLevel,
            AirQualityIndex = sensorData.AirQualityIndex,
            UptimeSeconds = sensorData.UptimeSeconds,
            RecordedAt = sensorData.RecordedAt,
            DeviceId = sensorData.DeviceId,
            DeviceCategoryName = sensorData.Device.DeviceCategory.Name
            // EnergyUsage = data.EnergyUsage
        };
    }

    public Device ToDevice(DeviceCreateRequest request,Location location, DeviceCategory category)
    {
       return new Device
        {
            IsActive = request.IsActive,
            SerialNumber = request.SerialNumber,
            InstalledAt = DateTime.UtcNow,
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
    }

    public Device ToDevice(DeviceUpdateRequest request, Device device)
    {
        device.SerialNumber = request.SerialNumber ?? device.SerialNumber;
        device.MACAddress = request.MACAddress ?? device.MACAddress;
        device.IsActive = request.IsActive ?? device.IsActive;
        
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

        return device;
    }

    public Location ToLocation(LocationCreateRequest request)
    {
       return new Location
        {
            Name = request.Name,
            Description = request.Description,
            FloorNumber = request.FloorNumber,
            RoomId = request.RoomId
        };
    }
}