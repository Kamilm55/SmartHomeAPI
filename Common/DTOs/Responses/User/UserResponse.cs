namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;

public class UserResponse
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? FullName { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    public List<UserDevicePermissionResponse> DevicePermissions { get; set; } = new();
}