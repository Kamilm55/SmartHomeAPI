using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Application.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(User user)
    {
        if (user == null) return null!;

        var deviceDtoSet = new HashSet<DeviceDTOInUserResponse>();
        foreach (var device in user.Devices)
        {
            var deviceDto = new DeviceDTOInUserResponse
            {
                Id = device.Id,
                IsActive = device.IsActive,
                InstalledAt = device.InstalledAt,
                SerialNumber = device.SerialNumber,
                
                DeviceCategoryName = device.DeviceCategory.Name,
                Manufacturer = device.DeviceCategory.Manufacturer
                
            }; 
            deviceDtoSet.Add(deviceDto);
        }

        return new UserResponse
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            FullName = user.FullName,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            LastLoginAt = user.LastLoginAt,
            Devices = deviceDtoSet
        };
    }

    public static User ToUser(UserCreateRequest request,string hashedPassword)
    {
        if (request == null) return null!;
       return new User
        {
           // Id = Guid.NewGuid(),
            UserName = request.Username,
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow,
            SecurityStamp = Guid.NewGuid().ToString(),
        };
    }
}
