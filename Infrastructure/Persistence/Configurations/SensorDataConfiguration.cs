using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence.Configurations;

public class SensorDataConfiguration : IEntityTypeConfiguration<SensorData>
{
    public void Configure(EntityTypeBuilder<SensorData> entity)
    {
        entity.HasKey(sd => sd.Id);

        // Required fields
        entity.Property(sd => sd.RecordedAt).IsRequired();
        
        // Required fields
        entity.Property(sd => sd.RecordedAt).IsRequired();
        
        // Optional fields
       // entity.Property(sd => sd.Voltage).HasPrecision(10, 2);
        //entity.Property(sd => sd.Current).HasPrecision(10, 2);
        entity.Property(sd => sd.PowerConsumptionWatts).HasPrecision(10, 2);

        entity.Property(sd => sd.BatteryLevel);
        entity.Property(sd => sd.SignalStrengthDb).HasPrecision(6, 2);
        entity.Property(sd => sd.Temperature).HasPrecision(5, 2);
        entity.Property(sd => sd.Humidity).HasPrecision(5, 2);
        entity.Property(sd => sd.Pressure).HasPrecision(6, 2);
        entity.Property(sd => sd.LightLevel).HasPrecision(6, 2);
        entity.Property(sd => sd.CO2Level).HasPrecision(6, 2);
        entity.Property(sd => sd.MotionDetected);
        entity.Property(sd => sd.SoundLevel).HasPrecision(6, 2);
        entity.Property(sd => sd.AirQualityIndex);
        entity.Property(sd => sd.UptimeSeconds);
            

        // Many-to-one relationship
        entity.HasOne(sd => sd.Device)
            .WithMany(d => d.SensorData)
            .HasForeignKey(sd => sd.DeviceId);
    }
}