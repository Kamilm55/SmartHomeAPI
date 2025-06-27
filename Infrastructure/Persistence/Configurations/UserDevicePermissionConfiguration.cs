/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart_Home_IoT_Device_Management_API.Domain.Entities.UserAndDevicePermission;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence.Configurations;

public class UserDevicePermissionConfiguration : IEntityTypeConfiguration<UserDevicePermission>
{
    public void Configure(EntityTypeBuilder<UserDevicePermission> entity)
    {
        // User 1 <---- many ---- UserDevicePermission ---- many ----> Device 1
        // UDP (User id:1, Device id:1 ,permission) -> User id:1  --> at the same time cannot be belonged to User id:2
        // UDP (User id:1, Device id:2 ,permission) -> User id:1
        
        // Composite key (UserId + DeviceId) prevents duplicate user-device pairs
        entity.HasKey(udp => new { udp.UserId, udp.DeviceId });

        // Relationship: Many UserDevicePermissions belong to one User
        entity.HasOne(udp => udp.User)
            .WithMany(u => u.UserDevicePermissions)
            .HasForeignKey(udp => udp.UserId)
            .OnDelete(DeleteBehavior.Cascade); 
        // When a User is deleted, related UserDevicePermissions are deleted too

        // Relationship: Many UserDevicePermissions belong to one Device
        entity.HasOne(udp => udp.Device)
            .WithMany(d => d.UserDevicePermissions)
            .HasForeignKey(udp => udp.DeviceId)
            .OnDelete(DeleteBehavior.Cascade); 
        // When a Device is deleted, related UserDevicePermissions are deleted too

        // Permission enum stored as string in DB
        entity.Property(udp => udp.Permission).IsRequired().HasConversion<string>();
    }
}*/