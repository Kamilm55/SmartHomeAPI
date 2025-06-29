using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Location;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public interface ILocationService
{
    Task<List<LocationResponse>> GetAllAsync();
    Task<LocationResponse> CreateAsync(LocationCreateRequest request);
    Task<List<DeviceInLocationResponse>> GetDevicesByLocationIdAsync(string id);
}