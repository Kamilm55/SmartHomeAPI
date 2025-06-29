using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(Guid id);
    Task<List<Location>> GetAllAsync();
    Task<Location?> GetByIdWithDevicesAndDeviceUsersAsync(Guid id);
    Task AddAsync(Location location);
    Task<bool> ExistsByIdAsync(Guid? requestLocationId);
    Task<List<Location>> GetAllByUserDevicesIdAsync(ICollection<Device> currentUserDevices);
}