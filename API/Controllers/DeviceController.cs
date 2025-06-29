using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Home_IoT_Device_Management_API.Application.Services;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Device;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Device;
using Smart_Home_IoT_Device_Management_API.Domain.Enum;

[ApiController]
[Route("api/v1/devices")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    /// <summary>
    /// Retrieves all devices associated with the authenticated user. If role is SuperAdmin retrieves all devices,
    /// If role is Admin retrieves the devices which belongs to the same home/admin
    /// </summary>
    /// <returns>List of devices</returns>
    /// <response code="200">Returns list of devices</response>
    /// <response code="401">Unauthorized - Token missing or invalid</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<List<DeviceResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<List<DeviceResponse>>>> GetAllDevices()
    {
        var devices = await _deviceService.GetAllDevicesAsync();
        return ApiResponse<List<DeviceResponse>>.Ok(devices);
    }

    /// <summary>
    /// Retrieves a device by its ID -> If it belongs to the current user
    /// </summary>
    /// <param name="id">Device ID</param>
    /// <returns>Device details</returns>
    /// <response code="200">Returns the device</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    ///  <response code="400">Not allowed to access this device</response>
    /// <response code="404">Device not found</response>
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<DeviceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<DeviceResponse>>> GetDeviceById(string id)
    {
        var device = await _deviceService.GetDeviceByIdAsync(id);
        return ApiResponse<DeviceResponse>.Ok(device);
    }

    /// <summary>
    /// Creates a new device. Only SuperAdmins can access this.
    /// </summary>
    /// <param name="request">Device creation data</param>
    /// <returns>The created device</returns>
    /// <response code="201">Device created successfully</response>
    /// <response code="400">Invalid input</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden - Only SuperAdmins allowed</response>
    [HttpPost]
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    [ProducesResponseType(typeof(ApiResponse<DeviceResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<DeviceResponse>>> CreateDevice([FromBody] DeviceCreateRequest request)
    {
        var created = await _deviceService.CreateDeviceAsync(request);
        return ApiResponse<DeviceResponse>.Created(created, $"/api/v1/devices/{created.Id}");
    }

    /// <summary>
    /// Updates an existing device. Allowed for SuperAdmin, Admin, or UserReadWrite roles.
    /// </summary>
    /// <param name="id">Device ID</param>
    /// <param name="request">Updated device data</param>
    /// <returns>Updated device</returns>
    /// <response code="200">Device updated successfully</response>
    /// <response code="400">Not allowed to access this device</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden - Not allowed to update</response>
    /// <response code="404">Device not found</response>
    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(Role.SuperAdmin)},{nameof(Role.Admin)},{nameof(Role.UserReadWrite)}")]
    [ProducesResponseType(typeof(ApiResponse<DeviceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<DeviceResponse>>> UpdateDevice(string id, [FromBody] DeviceUpdateRequest request)
    {
        var updated = await _deviceService.UpdateDeviceAsync(id, request);
        return ApiResponse<DeviceResponse>.Ok(updated, "Device updated successfully");
    }

    /// <summary>
    /// Deletes a device by its ID. Only SuperAdmins and Admins can access.
    /// </summary>
    /// <param name="id">Device ID</param>
    /// <returns>Confirmation message</returns>
    /// <response code="204">Device deleted successfully</response>
    /// <response code="400">Not allowed to access this device</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden - Not allowed to delete</response>
    /// <response code="404">Device not found</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{nameof(Role.SuperAdmin)},{nameof(Role.Admin)}")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<string>>> DeleteDevice(string id)
    {
        await _deviceService.DeleteDeviceAsync(id);
        return ApiResponse<string>.NoContent($"Device with id:{id} deleted successfully");
    }

    /// <summary>
    /// Assigns a device to an Admin. Only SuperAdmins can perform this action.
    /// </summary>
    /// <param name="id">Device ID</param>
    /// <param name="userId">Admin user ID</param>
    /// <returns>Assignment confirmation</returns>
    /// <response code="204">Device assigned to Admin</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden - Only SuperAdmin allowed</response>
    [HttpPatch("{id}/assign-to-admin")]
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<string>>> AssignDeviceToAdminRole(string id, [FromQuery] string userId)
    {
        await _deviceService.AssignDeviceToAdminRoleAsync(id, userId);
        return ApiResponse<string>.NoContent($"Device with id:{id} is assigned to admin with userId:{userId} successfully");
    }

    /// <summary>
    /// Assigns a device to a User. Only Admins can perform this action.
    /// </summary>
    /// <param name="id">Device ID</param>
    /// <param name="userId">User ID</param>
    /// <returns>Assignment confirmation</returns>
    /// <response code="204">Device assigned to User</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden - Only Admin allowed</response>
    [HttpPatch("{id}/assign-to-user")]
    [Authorize(Roles = nameof(Role.Admin))]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<string>>> AssignDeviceToUserRole(string id, [FromQuery] string userId)
    {
        await _deviceService.AssignDeviceToUserRoleAsync(id, userId);
        return ApiResponse<string>.NoContent($"Device with id:{id} is assigned to user with userId:{userId} successfully");
    }
}
