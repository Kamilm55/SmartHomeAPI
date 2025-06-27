namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;

public interface ISeedData
{
    Task InitializeAsync(SmartHomeContext context);
}