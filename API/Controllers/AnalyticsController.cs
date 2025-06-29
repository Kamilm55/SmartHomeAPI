using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Home_IoT_Device_Management_API.Application.Services;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Analytics;

[ApiController]
[Route("api/v1/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    /// <summary>
    /// Retrieves the total energy consumption of a specific device.
    /// </summary>
    /// <param name="deviceId">The ID of the device</param>
    /// <returns>Total energy usage data</returns>
    /// <response code="200">Returns total energy usage</response>
    /// <response code="401">Unauthorized - Token missing or invalid</response>
    /// <response code="404">Device not found</response>
    [HttpGet("devices/{id}/energy-usage")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<TotalEnergyUsageResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TotalEnergyUsageResponse>>> GetTotalEnergyUsage([FromRoute(Name = "id")] string deviceId)
    {
        var response = await _analyticsService.GetTotalEnergyUsageResponse(deviceId);
        return ApiResponse<TotalEnergyUsageResponse>.Ok(response);
    }

    /// <summary>
    /// Returns a summary of all devices belonging to the current user including active/offline status.
    /// </summary>
    /// <returns>Device status summary</returns>
    /// <response code="200">Returns device status summary</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet("device-status")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<DeviceStatusSummaryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<DeviceStatusSummaryResponse>>> GetDeviceStatusSummary()
    {
        var result = await _analyticsService.GetDeviceStatusSummaryAsync();
        return ApiResponse<DeviceStatusSummaryResponse>.Ok(result);
    }

    /// <summary>
    /// Returns usage statistics for a specific location.
    /// </summary>
    /// <param name="locationId">The ID of the location</param>
    /// <returns>Usage statistics for the location</returns>
    /// <response code="200">Returns usage statistics</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Location not found</response>
    [HttpGet("locations/{id}/usage")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<LocationUsageStatsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LocationUsageStatsResponse>>> GetUsageByLocation([FromRoute(Name = "id")] string locationId)
    {
        var result = await _analyticsService.GetUsageByLocationAsync(locationId);
        return ApiResponse<LocationUsageStatsResponse>.Ok(result);
    }

    /// <summary>
    /// Retrieves health status metrics of a device (e.g., battery level, signal strength).
    /// </summary>
    /// <param name="deviceId">The ID of the device</param>
    /// <returns>Device health report</returns>
    /// <response code="200">Returns device health metrics</response>
    /// <response code="404">Device not found</response>
    [HttpGet("devices/{id}/health")]
    [ProducesResponseType(typeof(ApiResponse<DeviceHealthStatusResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<DeviceHealthStatusResponse>>> GetDeviceHealthReport([FromRoute(Name = "id")] string deviceId)
    {
        var result = await _analyticsService.GetDeviceHealthStatusAsync(deviceId);
        return ApiResponse<DeviceHealthStatusResponse>.Ok(result);
    }
}
