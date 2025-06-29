namespace Smart_Home_IoT_Device_Management_API.Domain.Entities;

// Current sensor data of device (read-only)
// Example: Monitor environmental conditions: temperature, humidity, air quality
public class SensorData
{
    public Guid Id { get; set; }

    /// <summary>
    /// Real Time power consumption
    /// </summary>
    //public float? Voltage { get; set; }          // Volt
    //public float? Current { get; set; }          // Amper
    
    
    public float? PowerConsumptionWatts { get; set; } 
    
    public int? BatteryLevel { get; set; }
    
    /// <summary>
    /// Signal strength measured in decibels-milliwatts (dBm).
    /// Example: -45 (strong signal), -90 (weak signal)
    /// </summary>
    public float? SignalStrengthDb { get; set; }
    
    /// <summary>
    /// Temperature reading in degrees Celsius.
    /// Example: 22.5 (room temperature)
    /// </summary>
    public float? Temperature { get; set; }
    
    /// <summary>
    /// Humidity level as a percentage (0-100%).
    /// Example: 55.3 (55.3% relative humidity)
    /// </summary>
    public float? Humidity { get; set; }

    /// <summary>
    /// Atmospheric pressure measured in hectopascals (hPa).
    /// Example: 1013.25 (standard sea-level pressure)
    /// </summary>
    public float? Pressure { get; set; }

    /// <summary>
    /// Light intensity measured in lux.
    /// Example: 350 (typical indoor lighting)
    /// </summary>
    public float? LightLevel { get; set; }

    /// <summary>
    /// Carbon dioxide concentration in parts per million (ppm).
    /// Example: 400 (normal outdoor CO2 level)
    /// </summary>
    public float? CO2Level { get; set; }

    /// <summary>
    /// Motion detection status.
    /// Example: true (motion detected), false (no motion)
    /// </summary>
    public bool? MotionDetected { get; set; }

    /// <summary>
    /// Ambient sound level in decibels (dB).
    /// Example: 30.5 (quiet room), 85 (loud noise)
    /// </summary>
    public float? SoundLevel { get; set; }

    /// <summary>
    /// Air Quality Index (AQI) value indicating pollution level.
    /// Example: 42 (Good air quality)
    /// </summary>
    public int? AirQualityIndex { get; set; }

    /// <summary>
    /// Total uptime of the device in seconds.
    /// Example: 86400 (device has been running for 24 hours)
    /// </summary>
    public long? UptimeSeconds { get; set; }

    /// <summary>
    /// Timestamp when the sensor data was recorded.
    /// Example: 2025-06-22T15:00:00Z (UTC time)
    /// </summary>
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    
    // Many To One
    public Guid DeviceId  { get; set; }
    public Device Device { get; set; } = null!;
    
    public override string ToString()
    {
        return $"SensorData {{ " +
               $"Id = {Id}, " +
               //$"Voltage = {Voltage?.ToString() ?? "null"} V, " +
              // $"Current = {Current?.ToString() ?? "null"} A, " +
               $"PowerConsumptionWatts = {PowerConsumptionWatts?.ToString() ?? "null"} W, " +
               $"BatteryLevel = {BatteryLevel?.ToString() ?? "null"}%, " +
               $"SignalStrengthDb = {SignalStrengthDb?.ToString() ?? "null"} dBm, " +
               $"Temperature = {Temperature?.ToString() ?? "null"} °C, " +
               $"Humidity = {Humidity?.ToString() ?? "null"}%, " +
               $"Pressure = {Pressure?.ToString() ?? "null"} hPa, " +
               $"LightLevel = {LightLevel?.ToString() ?? "null"} lux, " +
               $"CO2Level = {CO2Level?.ToString() ?? "null"} ppm, " +
               $"MotionDetected = {MotionDetected?.ToString() ?? "null"}, " +
               $"SoundLevel = {SoundLevel?.ToString() ?? "null"} dB, " +
               $"AirQualityIndex = {AirQualityIndex?.ToString() ?? "null"}, " +
               $"UptimeSeconds = {UptimeSeconds?.ToString() ?? "null"} sec, " +
               $"RecordedAt = {RecordedAt:u}, " +
               $"DeviceId = {DeviceId} " +
               $"}}";
    }

    
}
