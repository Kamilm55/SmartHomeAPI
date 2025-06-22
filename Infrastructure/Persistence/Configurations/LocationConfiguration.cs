using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> entity)
    {
        entity.HasKey(l => l.Id);

        // Required fields
        entity.Property(l => l.Name).IsRequired().HasMaxLength(100);
        entity.Property(l => l.Description).HasMaxLength(250);

        // Optional fields
        entity.Property(l => l.FloorNumber);
        entity.Property(l => l.RoomId);

        // One-to-many relationship with Device
        entity.HasMany(l => l.Devices)
            .WithOne(d => d.Location)
            .HasForeignKey(d => d.LocationId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}