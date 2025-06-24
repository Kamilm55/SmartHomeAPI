using Microsoft.AspNetCore.Mvc;

namespace Smart_Home_IoT_Device_Management_API.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AnalyticsController : ControllerBase
{
    /*private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    /// <summary>
    /// Get total energy consumption across all devices
    /// </summary>
    [HttpGet("energy-usage")]
    public async Task<ActionResult<ApiResponse<TotalEnergyUsageResponse>>> GetTotalEnergyUsage()
    {
        var result = await _analyticsService.GetTotalEnergyUsageAsync();
        return ApiResponse<TotalEnergyUsageResponse>.Ok(result);
    }

    /// <summary>
    /// Get counts of online and offline devices
    /// </summary>
    [HttpGet("device-status")]
    public async Task<ActionResult<ApiResponse<DeviceStatusSummaryResponse>>> GetDeviceStatusSummary()
    {
        var result = await _analyticsService.GetDeviceStatusSummaryAsync();
        return ApiResponse<DeviceStatusSummaryResponse>.Ok(result);
    }

    /// <summary>
    /// Get usage statistics grouped by location
    /// </summary>
    [HttpGet("locations/usage")]
    public async Task<ActionResult<ApiResponse<List<LocationUsageStatsResponse>>>> GetUsageByLocation()
    {
        var result = await _analyticsService.GetUsageByLocationAsync();
        return ApiResponse<List<LocationUsageStatsResponse>>.Ok(result);
    }

    /// <summary>
    /// Get health status of devices (e.g. battery, signal, communication)
    /// </summary>
    [HttpGet("devices/health")]
    public async Task<ActionResult<ApiResponse<List<DeviceHealthStatusResponse>>>> GetDeviceHealthReport()
    {
        var result = await _analyticsService.GetDeviceHealthStatusAsync();
        return ApiResponse<List<DeviceHealthStatusResponse>>.Ok(result);
    }*/
}