using Microsoft.AspNetCore.Mvc;
using Smart_Home_IoT_Device_Management_API.Common;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Device;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Device;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

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

    // GET /api/v1/devices
    [HttpGet]
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
    public async Task<ActionResult<ApiResponse<object>>> DeleteDevice(string id)
    {
        await _deviceService.DeleteDeviceAsync(id);
        return ApiResponse<object>.NoContent($"Device with id:{id} deleted successfully");
    }
}