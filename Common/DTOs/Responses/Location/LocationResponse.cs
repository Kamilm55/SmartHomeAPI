namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;

public class LocationResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? FloorNumber { get; set; }
    public int? RoomId { get; set; }
}