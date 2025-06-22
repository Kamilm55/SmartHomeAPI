namespace Smart_Home_IoT_Device_Management_API.Domain.Entities;

public class Location
{
    public Guid Id { get; set; }
        
    public string Name { get; set; } = null!;  // e.g., "Living Room", "Kitchen", "Garage"
    
    public string? Description { get; set; }   // Optional details about the location
    public int? FloorNumber { get; set; }      // Floor as integer (e.g., 0 = Ground floor, 1 = First floor)
    public int? RoomId { get; set; }
    
    //public double? Latitude { get; set; }
    //public double? Longitude { get; set; }
    
    // Relationships
    // One To Many
    public ICollection<Device> Devices { get; set; } = new HashSet<Device>();
}
