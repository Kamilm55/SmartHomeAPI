using Microsoft.EntityFrameworkCore;
using Smart_Home_IoT_Device_Management_API.Common.Exceptions;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories;

public class DeviceCategoryRepository : IDeviceCategoryRepository
{
    private readonly SmartHomeContext _context;

    public DeviceCategoryRepository(SmartHomeContext context)
    {
        _context = context;
    }

    public async Task<DeviceCategory?> GetByIdAsync(Guid id)
    {
        return await _context.DeviceCategories.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsByIdAsync(Guid? id)
    {
        if (id == null)
            throw new ArgumentNullException( "Device category ID cannot be null.");

        var exists = await _context.Locations.AnyAsync(loc => loc.Id == id);

        if (!exists)
            throw new NotFoundException(nameof(DeviceCategory), id.Value);

        return true;
    }
    
}