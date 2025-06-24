using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.SensorData;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public interface ISensorReadingService
{
    Task<SensorDataResponse> AddReadingAsync(string deviceId, SensorDataRequest request);
    Task<List<SensorDataResponse>> GetAllReadingsAsync(string deviceId);
    Task<SensorDataResponse> GetLatestReadingAsync(string deviceId);
}