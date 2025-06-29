namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public interface IHelperService
{
    Task<User> getCurrentUserFromToken();
    Task IsThisDeviceBelongsToCurrentUser(Guid deviceId);
}