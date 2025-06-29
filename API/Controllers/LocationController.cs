using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Home_IoT_Device_Management_API.Application.Services;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Location;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Domain.Enum;

[ApiController]
[Route("api/v1/locations")]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    /// <summary>
    /// Retrieves all locations associated with the authenticated user.
    /// </summary>
    /// <returns>List of locations</returns>
    /// <response code="200">Returns all available locations</response>
    /// <response code="401">Unauthorized - Token is missing or invalid</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<List<LocationResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<List<LocationResponse>>>> GetAllLocations()
    {
        var locations = await _locationService.GetAllAsync();
        return ApiResponse<List<LocationResponse>>.Ok(locations, "All Locations");
    }

    /// <summary>
    /// Retrieves all devices in a specific location. If this location of device belongs to authenticated user
    /// </summary>
    /// <param name="id">The ID of the location</param>
    /// <returns>List of devices in the specified location</returns>
    /// <response code="200">Returns devices in the location</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden - Access denied to this location</response>
    /// <response code="404">Location not found</response>
    [HttpGet("{id}/devices")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<List<DeviceInLocationResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<List<DeviceInLocationResponse>>>> GetDevicesByLocationId(string id)
    {
        var devices = await _locationService.GetDevicesByLocationIdAsync(id);
        return ApiResponse<List<DeviceInLocationResponse>>.Ok(devices);
    }

    /// <summary>
    /// Creates a new location. Only SuperAdmins or Admins can create locations.
    /// </summary>
    /// <param name="request">The location creation request</param>
    /// <returns>The created location</returns>
    /// <response code="201">Location created successfully</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden - Insufficient permissions</response>
    [HttpPost]
    [Authorize(Roles = $"{nameof(Role.SuperAdmin)},{nameof(Role.Admin)}")]
    [ProducesResponseType(typeof(ApiResponse<LocationResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<LocationResponse>>> CreateLocation([FromBody] LocationCreateRequest request)
    {
        var created = await _locationService.CreateAsync(request);
        return ApiResponse<LocationResponse>.Created(created, $"/api/locations/{created.Id}");
    }
}
