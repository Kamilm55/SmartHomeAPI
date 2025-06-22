using Smart_Home_IoT_Device_Management_API.Domain.Entities.Owned;
using Smart_Home_IoT_Device_Management_API.Domain.Entities.UserAndDevicePermission;
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
    public Guid DeviceCategoryId  { get; set; }

    // One to Many (Device and UserDevicePermission) -> Many to Many (User and Device)
    public ICollection<UserDevicePermission> UserDevicePermissions { get; set; } = new HashSet<UserDevicePermission>();
    
    // One To One
    public SensorData SensorData { get; set; } = null!;
    
    // Many To One
    public Location Location { get; set; } = null!;
    public Guid LocationId { get; set; }
}
