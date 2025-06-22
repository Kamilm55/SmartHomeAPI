using System.ComponentModel.DataAnnotations;
using Smart_Home_IoT_Device_Management_API.Domain.Entities.UserAndDevicePermission;

namespace Smart_Home_IoT_Device_Management_API.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    public string? FullName { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    
    // Relationships
    // Which user has access to which devices at which level?  
   // User has 5 smart bulbs (DeviceCategory:same , Device:different)
   // (Example: id:1 User has access {deviceId:1,permission:read})
   
   // One to Many (User and UserDevicePermission) -> Many to Many (User and Device)
   public ICollection<UserDevicePermission> UserDevicePermissions { get; set; } = new HashSet<UserDevicePermission>();
}
