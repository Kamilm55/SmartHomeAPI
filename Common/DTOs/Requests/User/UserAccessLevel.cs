using System.Text.Json.Serialization;

namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserAccessLevel
{
    UserReadOnly,
    UserReadWrite
}