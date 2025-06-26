using Smart_Home_IoT_Device_Management_API.Application.Mappers;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.SensorData;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Common.Exceptions;
using Smart_Home_IoT_Device_Management_API.Common.Utils;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public class SensorReadingService : ISensorReadingService
{
    private readonly ISensorReadingRepository _repository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeviceService> _logger;

    public SensorReadingService(
        ISensorReadingRepository repository,
        IDeviceRepository deviceRepository,
        IMapper mapper,
        ILogger<DeviceService> logger,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _deviceRepository = deviceRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<SensorDataResponse> AddReadingAsync(string deviceId, SensorDataRequest request)
    {
        Guid guid = GuidParser.Parse(deviceId,nameof(Device));

        Device? device = await _deviceRepository.GetByIdWithSensorDataAsync(guid);
          if (device == null) throw new NotFoundException(nameof(Device), guid);
          
        
        SensorData? reading = new SensorData
        {
            Voltage = request.Voltage,
            Current = request.Current,
            PowerConsumptionWatts = request.PowerConsumptionWatts,
            BatteryLevel = request.BatteryLevel,
            SignalStrengthDb = request.SignalStrengthDb,
            Temperature = request.Temperature,
            Humidity = request.Humidity,
            Pressure = request.Pressure,
            LightLevel = request.LightLevel,
            CO2Level = request.CO2Level,
            MotionDetected = request.MotionDetected,
            SoundLevel = request.SoundLevel,
            AirQualityIndex = request.AirQualityIndex,
            UptimeSeconds = request.UptimeSeconds,
            DeviceId = device.Id
            //EnergyUsage = request.EnergyUsage
        };
        
        SensorData savedReading = await _repository.SaveChangesAndReturnLatestAsync(reading);

        _logger.LogCritical("SensorData:"  + reading.ToString());
        
        return _mapper.ToSensorDataResponse(savedReading);
    }

    public async Task<List<SensorDataResponse>> GetAllReadingsAsync(string deviceId)
    {
        Guid guid = GuidParser.Parse(deviceId,nameof(Device));

        var data = await _repository.GetAllByDeviceIdAsync(guid);
        return data.Select(_mapper.ToSensorDataResponse).ToList();
    }

    public async Task<SensorDataResponse> GetLatestReadingAsync(string deviceId)
    {
        Guid guid = GuidParser.Parse(deviceId,nameof(Device));

        var reading = await _repository.GetLatestByDeviceIdAsync(guid)
                      ?? throw new NotFoundException("Latest sensor reading not found");

        return _mapper.ToSensorDataResponse(reading);
    }
}