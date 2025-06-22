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
    public DateTime InstalledAt { get; set; }
    public string SerialNumber { get; set; } = null!;
    
    public float? PowerConsumption { get; set; } // This is the rated power listed by the manufacturer — usually found in the device datasheet or label
    public string? MACAddress { get; set; } = null!;
    public DateTime? LastCommunicationAt { get; set; }
    public int? UsageCount { get; set; }
    public DateTime? LastUsedAt { get; set; }
    
    // Owned
    public DeviceSetting DeviceSetting { get; set; } = null!;
    
    // Relationships
    // Many To One
    public DeviceCategory DeviceCategory { get; set; } = null!;

    // Many To Many
    public ICollection<User> User { get; set; } = new HashSet<User>(); 
    
    // One To Many
    public ICollection<DeviceSetting> DeviceSettings { get; set; } = new HashSet<DeviceSetting>();
    
    // One To One
    public SensorData SensorData { get; set; } = null!;
    
    // Many To One
    public Location Location { get; set; } = null!;
}
