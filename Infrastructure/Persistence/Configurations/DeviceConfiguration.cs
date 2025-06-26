using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence.Configurations;

 public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> entity)
    {
        entity.HasKey(d => d.Id);

        // Required fields
        entity.Property(d => d.IsActive).IsRequired();
        entity.Property(d => d.InstalledAt).IsRequired();
        entity.Property(d => d.SerialNumber).IsRequired().HasMaxLength(100);

        // Optional fields 
        entity.Property(d => d.PowerConsumption).HasPrecision(10, 2); // e.g., 50.75 watts
        entity.Property(d => d.MACAddress).HasMaxLength(50);
        entity.Property(d => d.LastCommunicationAt);
        entity.Property(d => d.UsageCount);
        entity.Property(d => d.LastUsedAt);

        // Many-to-One → DeviceCategory
        entity.HasOne(d => d.DeviceCategory)
            .WithMany(dc => dc.Devices)
            .HasForeignKey(d => d.DeviceCategoryId)
            .OnDelete(DeleteBehavior.Cascade); // When a DeviceCategory is deleted, its Devices are also deleted

        // Many-to-One → Location
        entity.HasOne(d => d.Location)
            .WithMany(l => l.Devices)
            .HasForeignKey(d => d.LocationId);

        // One-to-Many → SensorData
        entity.HasMany(d => d.SensorData)
            .WithOne(sd => sd.Device)
            .HasForeignKey(sd => sd.Id)
            .OnDelete(DeleteBehavior.Cascade);// Deleting Device deletes its SensorDatas

        // One-to-Many → UserDevicePermission
        entity.HasMany(d => d.UserDevicePermissions)
            .WithOne(udp => udp.Device)
            .HasForeignKey(udp => udp.DeviceId);

        entity.OwnsOne(d => d.DeviceSetting, ds =>
        {
            ds.Property(s => s.Brightness);
            ds.Property(s => s.Volume);
            ds.Property(s => s.TemperatureThreshold);
            ds.Property(s => s.AutoShutdown);
            ds.Property(s => s.MotionSensitivity);
            ds.Property(s => s.UpdateIntervalSeconds);
        });
    }
 }