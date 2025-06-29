using System.Text.Json.Serialization;

namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Analytics.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeviceHealthStatus
{
    Healthy = 0,        // Everything is normal
    Warning = 1,        // Minor issues (e.g., low battery)
    Critical = 2,       // Major issues (e.g., no communication)
    Offline = 3,        // Device is not currently connected
    Maintenance = 4     // Scheduled or in-progress maintenance
}