using Smart_Home_IoT_Device_Management_API.Domain.Enum;

namespace Smart_Home_IoT_Device_Management_API.Domain.Entities.UserAndDevicePermission;

public class UserDevicePermission
{
    public Guid Id { get; set; }

    // Foreign Keys
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    // Unique PermissionLevel per User-Device pair
    public PermissionLevel Permission { get; set; }
}
