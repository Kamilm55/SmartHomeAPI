using Microsoft.AspNetCore.Mvc;
using Smart_Home_IoT_Device_Management_API.Common;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.API.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class DevicesController : ControllerBase
{
    /*private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }*/

    // GET: /api/v1/devices
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
       // Device device1 = new Device(1L);
        //Device device2 = new Device(2L);
        
        List<Device> allDevices = new List<Device>();
            //await _deviceService.GetAllAsync();
        return Ok(ApiResponse<List<Device>>.Success(allDevices));
    }

    /*  // GET: /api/v1/devices/{id}
     [HttpGet("{id}")]
     public async Task<IActionResult> GetById(Guid id)
     {
         Device device = await _deviceService.GetByIdAsync(id);
         if (device == null)
             return NotFound(ApiResponse<object>.Fail("Device not found"));


         return Ok(ApiResponse<Device>.Success(device, $"Device with id:{id} retrieved successfully"));
     }

     // POST: /api/v1/devices
     [HttpPost]
     public async Task<IActionResult> Create([FromBody] DeviceCreateRequest request)
     {
         Device createdDevice = await _deviceService.CreateAsync(request);
         return CreatedAtAction(nameof(GetById), new { id = createdDevice.Id },
             ApiResponse<Device>.Success(createdDevice, $"Device with id:{createdDevice.Id} created successfully"));
     }

     // PUT: /api/v1/devices/{id}
    [HttpPut("{id}")]
     public async Task<IActionResult> Update(Guid id, [FromBody] DeviceUpdateRequest request)
     {
         Device updatedDevice = await _deviceService.UpdateAsync(id, request);
         if (updatedDevice == null)
             return NotFound(ApiResponse<object>.Fail("Device not found"));

         return Ok(ApiResponse<Device>.Success(updatedDevice, "Device updated successfully"));
     }

     // DELETE: /api/v1/devices/{id}
     [HttpDelete("{id}")]
     public async Task<IActionResult> Delete(Guid id)
     {
         var success = await _deviceService.DeleteAsync(id);
         if (!success)
             return NotFound(ApiResponse<object>.Fail("Device not found"));

         return NoContent(); // 204
     }*/
}