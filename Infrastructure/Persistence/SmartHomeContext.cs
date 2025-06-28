using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;


namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;

public class SmartHomeContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public SmartHomeContext(DbContextOptions<SmartHomeContext> options) : base(options) { }
    
    public DbSet<Device> Devices { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<SensorData> SensorDatas { get; set; }
    public DbSet<DeviceCategory> DeviceCategories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // When you inherit from IdentityDbContext<TUser, TRole, TKey>, you must call base.OnModelCreating(modelBuilder) inside your OnModelCreating override to let Identity configure the internal entities and keys.
        base.OnModelCreating(modelBuilder);
        
        // This auto-applies all classes that implement IEntityTypeConfiguration<>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartHomeContext).Assembly);
    }
}