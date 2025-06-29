using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Device;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Location;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.SensorData;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Application.Mappers;

public interface IMapper
{
    LocationResponse ToLocationResponse(Location location);
    DeviceInLocationResponse ToDeviceInLocationResponse(Device device);
    SensorData ToSensorData(SensorDataRequest request, Guid deviceId);
    SensorDataResponse ToSensorDataResponse(SensorData sensorData);
    Device ToDevice(DeviceCreateRequest request,Location location, DeviceCategory category);
    Device ToDevice(DeviceUpdateRequest request,Device device);
    Location ToLocation(LocationCreateRequest request);
}