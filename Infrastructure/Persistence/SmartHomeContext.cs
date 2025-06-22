using Microsoft.EntityFrameworkCore;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Domain.Entities.UserAndDevicePermission;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;

public class SmartHomeContext : DbContext
{
    public SmartHomeContext(DbContextOptions<SmartHomeContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<UserDevicePermission> UserDevicePermissions { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<SensorData> SensorDatas { get; set; }
    public DbSet<DeviceCategory> DeviceCategories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This auto-applies all classes that implement IEntityTypeConfiguration<>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartHomeContext).Assembly);
    }
}