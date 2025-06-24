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
            PowerConsumption = device.PowerConsumption,
            DeviceType = device.DeviceCategory.DeviceType,
            DeviceCategoryid = device.DeviceCategoryId,
            DeviceCategoryName = device.DeviceCategory.Name
        };
    }
    
    public SensorData ToSensorData(SensorDataRequest request)
    {
        return new SensorData
        {
            Temperature = request.Temperature,
            Humidity = request.Humidity,
            MotionDetected = request.MotionDetected
            //EnergyUsage = request.EnergyUsage
        };
    }

    public SensorDataResponse ToSensorDataResponse(SensorData data)
    {
        return new SensorDataResponse
        {
            Temperature = data.Temperature,
            Humidity = data.Humidity,
            MotionDetected = data.MotionDetected
           // EnergyUsage = data.EnergyUsage,
            //Timestamp = data.Timestamp
        };
    }
}