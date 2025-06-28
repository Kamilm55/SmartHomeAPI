using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Home_IoT_Device_Management_API.Application.Services;
using Smart_Home_IoT_Device_Management_API.Common;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Device;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Device;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Domain.Enum;

namespace Smart_Home_IoT_Device_Management_API.API.Controllers;


[ApiController]
[Route("api/v1/devices")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    // GET /api/v1/devices  --> All devices which belong to authorized user
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ApiResponse<List<DeviceResponse>>>> GetAllDevices()
    {
        List<DeviceResponse> devices = await _deviceService.GetAllDevicesAsync();
        return ApiResponse<List<DeviceResponse>>.Ok(devices);
    }

    // GET /api/v1/devices/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DeviceResponse>>> GetDeviceById(string id)
    {
        var deviceResponse = await _deviceService.GetDeviceByIdAsync(id);
        return ApiResponse<DeviceResponse>.Ok(deviceResponse);
    }

    // POST /api/v1/devices
    [HttpPost]
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    public async Task<ActionResult<ApiResponse<DeviceResponse>>> CreateDevice([FromBody] DeviceCreateRequest request)
    {
        DeviceResponse createdDevice = await _deviceService.CreateDeviceAsync(request);
        return ApiResponse<DeviceResponse>.Created(createdDevice, $"/api/v1/devices/{createdDevice.Id}");
    }

    // PUT /api/v1/devices/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<DeviceResponse>>> UpdateDevice(string id, [FromBody] DeviceUpdateRequest request)
    {
        DeviceResponse updatedDevice = await _deviceService.UpdateDeviceAsync(id, request);
        return ApiResponse<DeviceResponse>.Ok(updatedDevice, "Device updated successfully");
    }

    // DELETE /api/v1/devices/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteDevice(string id)
    {
        await _deviceService.DeleteDeviceAsync(id);
        return ApiResponse<string>.NoContent($"Device with id:{id} deleted successfully");
    }
    
    [HttpPatch("{id}/assign-to-admin")]
    [Authorize(Roles = nameof(Role.SuperAdmin))] // Only SuperAdmin allowed
    public async Task<ActionResult<ApiResponse<string>>> AssignDeviceToAdminRole(string id, [FromQuery] string userId)
    {
        await _deviceService.AssignDeviceToAdminRoleAsync(id,userId);
            
        return ApiResponse<string>.NoContent($"Device with id:{id} is assigned to admin with userId:{userId} successfully");
    }
    
    [HttpPatch("{id}/assign-to-user")]
    [Authorize(Roles = nameof(Role.Admin))] // Only Admin allowed
    public async Task<ActionResult<ApiResponse<string>>> AssignDeviceToUserRole(string id, [FromQuery] string userId)
    {
        await _deviceService.AssignDeviceToUserRoleAsync(id,userId);
            
        return ApiResponse<string>.NoContent($"Device with id:{id} is assigned to user with userId:{userId} successfully");
    }
    
} 