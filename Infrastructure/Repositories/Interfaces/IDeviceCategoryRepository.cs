using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

public interface IDeviceCategoryRepository
{
    Task<DeviceCategory?> GetByIdAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid? id);
}