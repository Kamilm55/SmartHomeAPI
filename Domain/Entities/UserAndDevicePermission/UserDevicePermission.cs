/*
using Smart_Home_IoT_Device_Management_API.Domain.Enum;

namespace Smart_Home_IoT_Device_Management_API.Domain.Entities.UserAndDevicePermission;

public class UserDevicePermission
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    // Many To One
    public User User { get; set; } = null!;

    public Guid DeviceId { get; set; }
    
    // Many To One
    public Device Device { get; set; } = null!;

    // Unique PermissionLevel per User-Device pair
    public PermissionLevel Permission { get; set; }
}
*/
