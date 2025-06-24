namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Location;

public class LocationCreateRequest
{
    public string Name { get; set; } = null!;  // Required (e.g., "Living Room", "Garage")
    public string? Description { get; set; }   // Optional extra info (e.g., "Main living area")
    public int? FloorNumber { get; set; }      // Optional (e.g., 0 = Ground, 1 = First floor)
    public int? RoomId { get; set; }           // Optional external room identifier
}