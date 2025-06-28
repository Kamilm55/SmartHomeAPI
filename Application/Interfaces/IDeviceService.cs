using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Device;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Device;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public interface IDeviceService
{
    Task<List<DeviceResponse>> GetAllDevicesAsync();
    Task<DeviceResponse> GetDeviceByIdAsync(string id);
    Task<DeviceResponse> CreateDeviceAsync(DeviceCreateRequest request);
    Task<DeviceResponse> UpdateDeviceAsync(string id, DeviceUpdateRequest request);
    Task DeleteDeviceAsync(string id);
    Task AssignDeviceToAdminRoleAsync(string id, string userId);
    Task AssignDeviceToUserRoleAsync(string id, string userId);
}