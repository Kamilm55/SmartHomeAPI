using Microsoft.EntityFrameworkCore;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly SmartHomeContext _context;

    public DeviceRepository(SmartHomeContext context)
    {
        _context = context;
    }

    public async Task<List<Device>> GetAllByUserIdWithCategoryAndLocationAsync(Guid userId)
    {
        return await _context.Devices
            .Where(d => d.Users.Any(u => u.Id == userId))
            .Include(d => d.DeviceCategory)
            .Include(d => d.DeviceSetting)
            .Include(d => d.Location)
            .ToListAsync();
    }

   

    public async Task<List<Device>> GetAllWithCategoryAndLocationAsync()
    {
        return await _context.Devices
            .Include(d => d.DeviceCategory)
            .Include(d => d.DeviceSetting)
            .Include(d => d.Location)
            .ToListAsync();
    }

    public async Task<Device?> GetByIdWithCategoryAndLocationAsync(Guid id)
    {
        return await _context.Devices
            .Include(d => d.DeviceCategory)
            .Include(d => d.DeviceSetting)
            .Include(d => d.Location)
            .FirstOrDefaultAsync(d => d.Id == id);
    }
    public async Task<Device?> GetByIdAndByUserIdWithCategoryAndLocationAsync(Guid deviceId, Guid userId)
    {
        return await _context.Devices
            .Include(d => d.DeviceCategory)
            .Include(d => d.DeviceSetting)
            .Include(d => d.Location)
            .FirstOrDefaultAsync(
                d => d.Id == deviceId && d.Users.Any(u => u.Id == userId)
            );
        
    }

    public async Task AddAsync(Device device)
    {
        await _context.Devices.AddAsync(device);
    }

    public Task Update(Device device)
    {
        _context.Devices.Update(device);
        return Task.CompletedTask;
    }

    public Task Delete(Device device)
    {
        _context.Devices.Remove(device);
        return Task.CompletedTask;
    }

    
    public async Task<Device?> SaveDeviceAndReturnLatest(Device device)
    {
        if (device == null)
            throw new ArgumentNullException(nameof(device));

        // Save changes
        await _context.SaveChangesAsync();

        // Re-fetch with all required navigation properties
        var savedDevice = await _context.Devices
            .Include(d => d.DeviceCategory)
            .Include(d => d.Location)
            .Include(d => d.DeviceSetting)
            .FirstOrDefaultAsync(d => d.Id == device.Id);

        if (savedDevice == null)
            throw new InvalidOperationException($"Device with ID {device.Id} was not found after saving");


        return savedDevice;
    }

    public async Task<Device?> GetByIdWithSensorDataAsync(Guid deviceId)
    {
        return await _context.Devices
            .Include(d => d.SensorData)
            .FirstOrDefaultAsync(d => d.Id == deviceId);
    }

    public async Task<Device?> GetByIdAsync(Guid deviceId)
    {
        return await _context.Devices
            .FirstOrDefaultAsync(d => d.Id == deviceId);
    }

    public async Task<List<Device>> GetAllDevicesAsync()
    {
       return await _context.Devices
           .Include(d => d.DeviceCategory)
           .ToListAsync();
    }

    
}
