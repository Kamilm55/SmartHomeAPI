using Microsoft.AspNetCore.Mvc;
using Smart_Home_IoT_Device_Management_API.Application.Services;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Location;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;

namespace Smart_Home_IoT_Device_Management_API.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    // GET: /api/v1/locations
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<LocationResponse>>>> GetAllLocations()
    {
        var locations = await _locationService.GetAllAsync();
        return ApiResponse<List<LocationResponse>>.Ok(locations,"All Locations");
    }

    // GET: /api/v1/locations/{id}/devices
    [HttpGet("{id}/devices")]
    public async Task<ActionResult<ApiResponse<List<DeviceInLocationResponse>>>> GetDevicesByLocationId(string id)
    {
        var devices = await _locationService.GetDevicesByLocationIdAsync(id);
        return ApiResponse<List<DeviceInLocationResponse>>.Ok(devices);
    }

    // POST: /api/v1/locations
    [HttpPost]
    public async Task<ActionResult<ApiResponse<LocationResponse>>> CreateLocation([FromBody] LocationCreateRequest request)
    {
        LocationResponse created = await _locationService.CreateAsync(request);
        return ApiResponse<LocationResponse>.Created(created, $"/api/locations/{created.Id}");
    }
}