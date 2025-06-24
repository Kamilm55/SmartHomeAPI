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
            .OrderByDescending(sd => sd.RecordedAt)
            .ToListAsync();
    }

    public async Task<SensorData?> GetLatestByDeviceIdAsync(Guid deviceId)
    {
        return await _context.SensorDatas
            .Where(sd => sd.Device.Id == deviceId)
            .OrderByDescending(sd => sd.RecordedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<SensorData> SaveChangesAndReturnLatestAsync(SensorData reading)
    {
        await _context.SensorDatas.AddAsync(reading);
        
        if (reading == null)
            throw new ArgumentNullException(nameof(reading));

        // Save changes
        await _context.SaveChangesAsync();

        var savedSensorData = await _context.SensorDatas
            .Include(sd => sd.Device)
            .FirstOrDefaultAsync(sd => sd.Id == reading.Id);

        if (savedSensorData == null)
            throw new InvalidOperationException($"Sensor Data with ID {savedSensorData.Id} was not found after saving");

        return savedSensorData;
    }
}