using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Home_IoT_Device_Management_API.Application.Services;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.SensorData;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Domain.Enum;

[ApiController]
[Route("api/v1/devices/{id}/readings")]
public class SensorReadingController : ControllerBase
{
    private readonly ISensorReadingService _sensorService;

    public SensorReadingController(ISensorReadingService sensorService)
    {
        _sensorService = sensorService;
    }

    /// <summary>
    /// Adds a new sensor reading for the specified device -> If device belong to the authenticated user.
    /// </summary>
    /// <param name="deviceId">The ID of the device</param>
    /// <param name="request">Sensor data to be added</param>
    /// <returns>The created sensor reading</returns>
    /// <response code="201">Sensor reading created successfully</response>
    /// <response code="400">Not allowed to access this device</response>
    /// <response code="401">Unauthorized - Token is missing or invalid</response>
    /// <response code="403">Forbidden - Insufficient role to add readings</response>
    [HttpPost]
    [Authorize(Roles = $"{nameof(Role.SuperAdmin)},{nameof(Role.Admin)},{nameof(Role.UserReadWrite)}")]
    [ProducesResponseType(typeof(ApiResponse<SensorDataResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<SensorDataResponse>>> AddReading(
        [FromRoute(Name = "id")] string deviceId,
        [FromBody] SensorDataRequest request)
    {
        var reading = await _sensorService.AddReadingAsync(deviceId, request);
        return ApiResponse<SensorDataResponse>.Created(reading, $"api/devices/{deviceId}/readings/latest");
    }

    /// <summary>
    /// Retrieves all sensor readings for a specific device.
    /// </summary>
    /// <param name="deviceId">The ID of the device</param>
    /// <returns>List of sensor readings</returns>
    /// <response code="200">Returns all readings for the device</response>
    /// <response code="400">Not allowed to access this device</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<List<SensorDataResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<List<SensorDataResponse>>>> GetAllReadings(
        [FromRoute(Name = "id")] string deviceId)
    {
        var readings = await _sensorService.GetAllReadingsAsync(deviceId);
        return ApiResponse<List<SensorDataResponse>>.Ok(readings);
    }

    /// <summary>
    /// Retrieves the latest sensor reading for a specific device.
    /// </summary>
    /// <param name="deviceId">The ID of the device</param>
    /// <returns>The most recent sensor reading</returns>
    /// <response code="200">Returns the latest sensor reading</response>
    /// <response code="400">Not allowed to access this device</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">No readings found for the device</response>
    [HttpGet("latest")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<SensorDataResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SensorDataResponse>>> GetLatestReading(
        [FromRoute(Name = "id")] string deviceId)
    {
        var reading = await _sensorService.GetLatestReadingAsync(deviceId);
        return ApiResponse<SensorDataResponse>.Ok(reading);
    }
}
