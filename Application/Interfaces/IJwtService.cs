using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}