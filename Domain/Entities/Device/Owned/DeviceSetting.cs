namespace Smart_Home_IoT_Device_Management_API.Domain.Entities.Owned;

// Manually to change settings
// Example: Be configured by the user: target temperature, schedule, eco mode etc.
public class DeviceSetting
{
    public Guid Id { get; set; }
    
    public int? Brightness { get; set; }           // e.g., 0 - 100%
    public int? Volume { get; set; }               // e.g., 0 - 100%
    public int? TemperatureThreshold { get; set; } // e.g., Celsius, default 25
    public bool? AutoShutdown { get; set; }        // e.g., true/false
    
    public int? MotionSensitivity { get; set; }  // nullable, only for motion sensors
    public int? UpdateIntervalSeconds { get; set; } // nullable, frequency in seconds
    
    //  public User? LastUpdatedBy { get; set; }
    
}
