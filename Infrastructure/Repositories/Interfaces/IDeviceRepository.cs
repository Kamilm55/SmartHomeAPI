using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

public interface IDeviceRepository
{
    Task Delete(Device device); 
    Task<Device?> GetByIdWithCategoryAndLocationAsync(Guid id);
    Task<Device?> GetByIdWithSensorDataAsync(Guid deviceId);
    Task<Device?> GetByIdAsync(Guid deviceId);
    Task<List<Device>> GetAllByUserIdWithCategoryAndLocationAsync(Guid userId);
    Task AddAsync(Device device);
    Task Update(Device device);
    Task<List<Device>> GetAllDevicesAsync();
    Task<List<Device>> GetAllWithCategoryAndLocationAsync();
    Task<Device?> GetByIdAndByUserIdWithCategoryAndLocationAsync(Guid deviceId,Guid userId);
}