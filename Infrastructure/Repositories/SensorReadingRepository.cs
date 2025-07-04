using Microsoft.EntityFrameworkCore;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

public class SensorReadingRepository : ISensorReadingRepository
{
    private readonly SmartHomeContext _context;

    public SensorReadingRepository(SmartHomeContext context)
    {
        _context = context;
    }

    public async Task AddAsync(SensorData data)
    {
        
    }

    public async Task<List<SensorData>> GetAllByDeviceIdAsync(Guid deviceId)
    {
        return await _context.SensorDatas
            .Where(sd => sd.Device.Id == deviceId)
            .Include(sd => sd.Device)
            .Include(sd => sd.Device.DeviceCategory)
            .OrderByDescending(sd => sd.RecordedAt)
            .ToListAsync();
    }

    public async Task<SensorData?> GetLatestByDeviceIdAsync(Guid deviceId)
    {
        return await _context.SensorDatas
            .Where(sd => sd.Device.Id == deviceId)
            .Include(sd => sd.Device)
            .Include(sd => sd.Device.DeviceCategory)
            .OrderByDescending(sd => sd.RecordedAt)
            .FirstOrDefaultAsync();
    }

    

    public async Task<List<SensorData>> GetAllByDevicesAndByLocationAsync(ICollection<Device> currentUserDevices,Guid locationId)
    {
        var deviceIdList = currentUserDevices.Select(d => d.Id).ToList();

        return await _context.SensorDatas
            .Where(sd => deviceIdList.Contains(sd.DeviceId) && sd.Device.Location.Id == locationId)
            .ToListAsync();
    }

}