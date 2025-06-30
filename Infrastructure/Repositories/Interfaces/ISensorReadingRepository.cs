using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

public interface ISensorReadingRepository
{
    Task AddAsync(SensorData data);
    Task<List<SensorData>> GetAllByDeviceIdAsync(Guid deviceId);
    Task<SensorData?> GetLatestByDeviceIdAsync(Guid deviceId);
    Task<List<SensorData>> GetAllByDevicesAndByLocationAsync(ICollection<Device> currentUserDevices,Guid locationId);
}