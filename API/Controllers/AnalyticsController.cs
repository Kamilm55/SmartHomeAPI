using Microsoft.AspNetCore.Mvc;
using Smart_Home_IoT_Device_Management_API.Application.Services;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Analytics;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    // Total energy consumption from all sensor data for specific device
    [HttpGet("devices/{id}/energy-usage")]
    public async Task<ActionResult<ApiResponse<TotalEnergyUsageResponse>>> GetTotalEnergyUsage([FromRoute(Name = "id")] string deviceId)
    {
        TotalEnergyUsageResponse response = await _analyticsService.GetTotalEnergyUsageResponse(deviceId);
        
        return ApiResponse<TotalEnergyUsageResponse>.Ok(response);
    }

    // Returns a summary of device statuses, including:
    // - Total count of active and not active devices
    // - A list of all devices with their ID, name, and current active/offline status
    [HttpGet("device-status")]
    public async Task<ActionResult<ApiResponse<DeviceStatusSummaryResponse>>> GetDeviceStatusSummary()
    {
        var result = await _analyticsService.GetDeviceStatusSummaryAsync();
        return ApiResponse<DeviceStatusSummaryResponse>.Ok(result);
    }

    // Usage statistics per location 
    /*[HttpGet("locations/{id}/usage")]
    public async Task<ActionResult<ApiResponse<List<LocationUsageStatsResponse>>>> GetUsageByLocation([FromRoute(Name = "id")] string locationId)
    {
        /*
         *  var groupedUsage = _sensorDataRepository.GetAll() 
            .Where(s => s.PowerConsumptionWatts.HasValue)
            .GroupBy(s => s.Location)
            .Select(g => new {
                Location = g.Key,
                TotalPower = g.Sum(s => s.PowerConsumptionWatts.Value)
            }).ToList();
         #1#
        var result = await _analyticsService.GetUsageByLocationAsync();
        return ApiResponse<List<LocationUsageStatsResponse>>.Ok(result);
    }

    // Show metrics like battery level, signal strength
    [HttpGet("devices/health")]
    public async Task<ActionResult<ApiResponse<List<DeviceHealthStatusResponse>>>> GetDeviceHealthReport()
    {
        /*
         * var data = _sensorDataRepository.GetLatestPerDevice(); // latest reading per device

            var report = data.Select(s => new {
                DeviceId = s.DeviceId,
                BatteryLevel = s.BatteryLevel,
                SignalStrengthDb = s.SignalStrengthDb,
                IsOnline = s.IsOnline
            }).ToList();

         #1#
        var result = await _analyticsService.GetDeviceHealthStatusAsync();
        return ApiResponse<List<DeviceHealthStatusResponse>>.Ok(result);
    }*/
}