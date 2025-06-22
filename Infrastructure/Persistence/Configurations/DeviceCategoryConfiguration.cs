using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence.Configurations;

public class DeviceCategoryConfiguration : IEntityTypeConfiguration<DeviceCategory>
{
    public void Configure(EntityTypeBuilder<DeviceCategory> entity)
    {
        entity.HasKey(dc => dc.Id);

        entity.Property(dc => dc.Name).IsRequired().HasMaxLength(100);
        entity.Property(dc => dc.Manufacturer).IsRequired().HasMaxLength(100);
        entity.Property(dc => dc.FirmwareVersion).IsRequired().HasMaxLength(50);

        // Enum - stored as string
        entity.Property(dc => dc.PowerSource).IsRequired().HasConversion<string>(); 

        entity.Property(dc => dc.RequiresInternet)
            .IsRequired();

        // Optional fields
        entity.Property(dc => dc.Description).HasMaxLength(250);
        entity.Property(dc => dc.CommunicationProtocol).HasMaxLength(50);
        entity.Property(dc => dc.DeviceType).HasConversion<int>(); // enum stored as int

        // Relationships
        entity.HasMany(dc => dc.Devices)
            .WithOne(d => d.DeviceCategory)
            .HasForeignKey(d => d.DeviceCategoryId)
            .OnDelete(DeleteBehavior.Cascade); // If a DeviceCategory is deleted, delete all related Devices automatically;
    }
}