using Microsoft.EntityFrameworkCore;
using Smart_Home_IoT_Device_Management_API.Common.Exceptions;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly SmartHomeContext _context;

    public LocationRepository(SmartHomeContext context)
    {
        _context = context;
    }

    public async Task<Location?> GetByIdAsync(Guid id)
    {
        return await _context.Locations.FirstOrDefaultAsync(l => l.Id == id);
    }
    public async Task<List<Location>> GetAllAsync()
    {
        return await _context.Locations
            .ToListAsync();
    }

    public async Task<Location?> GetByIdWithDevicesAndDeviceUsersAsync(Guid id)
    {
        return await _context.Locations
            .Include(l => l.Devices)
            .ThenInclude(d => d.DeviceCategory)
            
            .Include(l => l.Devices)
            .ThenInclude(d => d.Users)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task AddAsync(Location location)
    {
        await _context.Locations.AddAsync(location);
    }
    
    public async Task<bool> ExistsByIdAsync(Guid? locationId)
    {
        if (locationId == null)
            throw new ArgumentNullException( "Location ID cannot be null.");

        var exists = await _context.Locations.AnyAsync(loc => loc.Id == locationId);

        if (!exists)
            throw new NotFoundException(nameof(Location), locationId.Value);

        return true;
    }

    public async Task<List<Location>> GetAllByUserDevicesIdAsync(ICollection<Device> currentUserDevices)
    {
       var deviceIds = currentUserDevices.Select(d => d.Id).ToList();
       return await _context.Locations
                               .Where(l => l.Devices.Any(d => deviceIds.Contains(d.Id)))
                               .ToListAsync();
    }
}