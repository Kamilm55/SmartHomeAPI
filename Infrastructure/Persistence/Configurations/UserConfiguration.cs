using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(u => u.Id);
        
        // Unique fields
        entity.HasIndex(u => u.UserName).IsUnique();
        entity.HasIndex(u => u.Email).IsUnique();

        // Required fields
        entity.Property(u => u.UserName).IsRequired().HasMaxLength(30);
        entity.Property(u => u.Email).IsRequired().HasMaxLength(50);
        entity.Property(u => u.CreatedAt).IsRequired();
        entity.Property(u => u.UpdatedAt).IsRequired(false);
        entity.Property(u => u.LastLoginAt).IsRequired(false);

        // Optional fields
        entity.Property(u => u.FullName).HasMaxLength(200);

        entity.HasMany(u => u.Devices)
            .WithMany(d => d.Users);
        
    }
}