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

        _logger.LogInformation("LOG 0");
        Device? device = await _deviceRepository.GetByIdWithSensorDataAsync(guid);
        _logger.LogCritical("Device: " + device.ToString());
          if (device == null) throw new NotFoundException(nameof(Device), guid);
          
          
        SensorData? reading = _mapper.ToSensorData(request);
        _logger.LogInformation(reading.ToString());
        reading.Device.Id = device.Id;
        
        
        SensorData savedReading = await _repository.SaveChangesAndReturnLatestAsync(reading);

        return _mapper.ToSensorDataResponse(new SensorData());
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