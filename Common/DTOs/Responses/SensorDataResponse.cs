namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;


public class SensorDataResponse
{
    public Guid Id { get; set; }
    public float? Voltage { get; set; }
    public float? Current { get; set; }
    public float? PowerConsumptionWatts { get; set; }
    public int? BatteryLevel { get; set; }
    public float? SignalStrengthDb { get; set; }
    public float? Temperature { get; set; }
    public float? Humidity { get; set; }
    public float? Pressure { get; set; }
    public float? LightLevel { get; set; }
    public float? CO2Level { get; set; }
    public bool? MotionDetected { get; set; }
    public float? SoundLevel { get; set; }
    public int? AirQualityIndex { get; set; }
    public long? UptimeSeconds { get; set; }
    public DateTime RecordedAt { get; set; }
    public Guid DeviceId { get; set; }
    public string DeviceCategoryName { get; set; }
}