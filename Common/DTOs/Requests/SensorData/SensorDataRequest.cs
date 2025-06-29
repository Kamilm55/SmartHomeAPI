using System.ComponentModel.DataAnnotations;

namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.SensorData;

public class SensorDataRequest
{
    [Range(0, double.MaxValue, ErrorMessage = "PowerConsumptionWatts cannot be negative.")]
    public float PowerConsumptionWatts { get; set; } = 0;

    [Range(0, 100, ErrorMessage = "BatteryLevel must be between 0 and 100.")]
    public int BatteryLevel { get; set; } = 0;

    [Range(-150, 0, ErrorMessage = "SignalStrengthDb must be between -150 and 0 dB.")]
    public float SignalStrengthDb { get; set; } = 0;

    [Range(-50, 150, ErrorMessage = "Temperature must be between -50 and 150 °C.")]
    public float Temperature { get; set; } = 0;

    [Range(0, 100, ErrorMessage = "Humidity must be between 0 and 100%.")]
    public float Humidity { get; set; } = 0;

    [Range(300, 1100, ErrorMessage = "Pressure must be between 300 and 1100 hPa.")]
    public float Pressure { get; set; } = 0;

    [Range(0, 100000, ErrorMessage = "LightLevel must be between 0 and 100000 lux.")]
    public float LightLevel { get; set; } = 0;

    [Range(0, 5000, ErrorMessage = "CO2Level must be between 0 and 5000 ppm.")]
    public float CO2Level { get; set; } = 0;

    public bool MotionDetected { get; set; } = false;

    [Range(0, 200, ErrorMessage = "SoundLevel must be between 0 and 200 dB.")]
    public float SoundLevel { get; set; } = 0;

    [Range(0, 500, ErrorMessage = "AirQualityIndex must be between 0 and 500.")]
    public int AirQualityIndex { get; set; } = 0;

    [Range(0, long.MaxValue, ErrorMessage = "UptimeSeconds cannot be negative.")]
    public long UptimeSeconds { get; set; } = 0;
}