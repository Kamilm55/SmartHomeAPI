using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.SensorData;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

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
            Temperature = request.Temperature,
            Humidity = request.Humidity,
            MotionDetected = request.MotionDetected,
            DeviceId = deviceId
            //EnergyUsage = request.EnergyUsage
        };
    }

    public SensorDataResponse ToSensorDataResponse(SensorData sensorData)
    {
       return new SensorDataResponse
        {
            Id = sensorData.Id,
           // Voltage = sensorData.Voltage,
           // Current = sensorData.Current,
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
}