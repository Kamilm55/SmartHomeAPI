using Microsoft.AspNetCore.Mvc;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.SensorData;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;

namespace Smart_Home_IoT_Device_Management_API.API.Controllers;

[ApiController]
[Route("api/v1/devices/{id}/readings")]
public class SensorReadingController : ControllerBase
{
    private readonly ISensorReadingService _sensorService;

    public SensorReadingController(ISensorReadingService sensorService)
    {
        _sensorService = sensorService;
    }

    // POST /api/v1/devices/{id}/readings
    // Use Case:
    // A smart thermostat sends a temperature reading (23.5°C) to your API every 10 minutes.
    [HttpPost]
    public async Task<ActionResult<ApiResponse<SensorDataResponse>>> AddReading(string deviceId, SensorDataRequest request)
    {
        var reading = await _sensorService.AddReadingAsync(deviceId, request);
        return ApiResponse<SensorDataResponse>.Created(reading, $"api/devices/{deviceId}/readings/latest");
    }

    // GET /api/v1/devices/{id}/readings
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SensorDataResponse>>>> GetAllReadings(string deviceId)
    {
        var readings = await _sensorService.GetAllReadingsAsync(deviceId);
        return ApiResponse<List<SensorDataResponse>>.Ok(readings);
    }

    // GET /api/v1/devices/{id}/readings/latest
    [HttpGet("latest")]
    public async Task<ActionResult<ApiResponse<SensorDataResponse>>> GetLatestReading(string deviceId)
    {
        var reading = await _sensorService.GetLatestReadingAsync(deviceId);
        return ApiResponse<SensorDataResponse>.Ok(reading);
    }
}