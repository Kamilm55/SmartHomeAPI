namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public interface IAuthService
{
    Task<User> getCurrentUserFromToken();
}