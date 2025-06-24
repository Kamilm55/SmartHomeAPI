using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Application.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(User user)
    {
        if (user == null) return null!;

        return new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            LastLoginAt = user.LastLoginAt,
            DevicePermissions = user.UserDevicePermissions.Select(udp => new UserDevicePermissionResponse
            {
                DeviceId = udp.DeviceId,
                SerialNumber = udp.Device.SerialNumber,
                DeviceCategory = udp.Device.DeviceCategory.Name,
                DeviceTypeGroup = udp.Device.DeviceCategory.DeviceType,
                LocationName = udp.Device.Location.Name,
                Permission = udp.Permission
            }).ToList()
        };
    }
}
