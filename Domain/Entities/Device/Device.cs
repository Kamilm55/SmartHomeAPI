using Smart_Home_IoT_Device_Management_API.Domain.Entities.Owned;
using Smart_Home_IoT_Device_Management_API.Domain.Enum;

namespace Smart_Home_IoT_Device_Management_API.Domain.Entities;

// Every device has unique id, not for category type: 
// User has 5 smart bulbs (DeviceCategory:same , Device:different)

// One Device can belong to many Users
// One User can own or have access to many Devices
public class Device
{
    public Guid Id { get; set; }
    
    public bool IsActive { get; set; } = true; //  device is working or not
    public DateTime InstalledAt { get; set; } = DateTime.UtcNow;
    public string SerialNumber { get; set; } = null!;
    
    public string? MACAddress { get; set; } = null!;
    
    // Optional
    public DateTime? LastCommunicationAt { get; set; }
    
    // Owned
    public DeviceSetting DeviceSetting { get; set; } = null!;
    
    // Relationships
    // Many To One
    public DeviceCategory DeviceCategory { get; set; } = null!;
    public Guid DeviceCategoryId  { get; set; }

    //  Many to Many (User and Device)
    public ICollection<User> Users { get; set; } = new HashSet<User>();
    
    // One To Many
    public ICollection<SensorData> SensorData { get; set; } = new HashSet<SensorData>();
    
    // Many To One
    public Location Location { get; set; } = null!;
    public Guid LocationId { get; set; }
    
    public override string ToString()
    {
        return $"Device {{ " +
               $"Id = {Id}, " +
               $"IsActive = {IsActive}, " +
               $"InstalledAt = {InstalledAt}, " +
               $"SerialNumber = {SerialNumber}, " +
               $"MACAddress = {MACAddress ?? "null"}, " +
               $"LastCommunicationAt = {LastCommunicationAt?.ToString() ?? "null"}, " +
               $"DeviceSetting = {DeviceSetting?.ToString() ?? "null"}, " +
               $"DeviceCategoryId = {DeviceCategoryId}, " +
               $"LocationId = {LocationId}, " +
               $"Users.Count = {Users?.Count ?? 0}, " +
               $"SensorData.Count = {SensorData?.Count ?? 0} " +
               $"}}";
    }


    
}
