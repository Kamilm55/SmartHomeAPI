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
    private readonly IHelperService _helperService;

    public SensorReadingService(
        ISensorReadingRepository repository,
        IDeviceRepository deviceRepository,
        IMapper mapper,
        ILogger<DeviceService> logger,
        IUnitOfWork unitOfWork, IHelperService helperService)
    {
        _repository = repository;
        _deviceRepository = deviceRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _helperService = helperService;
        _logger = logger;
    }
 public async Task<SensorDataResponse> AddReadingAsync(string deviceId, SensorDataRequest request)
    {
        var device = await GetDeviceWithValidationAsync(deviceId);

        var reading = _mapper.ToSensorData(request, device.Id);

        await _repository.AddAsync(reading);
        device.LastCommunicationAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToSensorDataResponse(reading);
    }

    public async Task<List<SensorDataResponse>> GetAllReadingsAsync(string deviceId)
    {
        var guid = await ValidateAccessAndParseGuidAsync(deviceId);
        var data = await _repository.GetAllByDeviceIdAsync(guid);
        return data.Select(_mapper.ToSensorDataResponse).ToList();
    }

    public async Task<SensorDataResponse> GetLatestReadingAsync(string deviceId)
    {
        var guid = await ValidateAccessAndParseGuidAsync(deviceId);

        var reading = await _repository.GetLatestByDeviceIdAsync(guid)
                      ?? throw new NotFoundException("Latest sensor reading not found");

        return _mapper.ToSensorDataResponse(reading);
    }

    // Helpers

    private async Task<Device> GetDeviceWithValidationAsync(string deviceId)
    {
        var guid = GuidParser.Parse(deviceId, nameof(Device));

        var device = await _deviceRepository.GetByIdWithSensorDataAsync(guid)
                     ?? throw new NotFoundException(nameof(Device), guid);

        await _helperService.IsThisDeviceBelongsToCurrentUser(guid);

        return device;
    }

    private async Task<Guid> ValidateAccessAndParseGuidAsync(string deviceId)
    {
        var guid = GuidParser.Parse(deviceId, nameof(Device));
        await _helperService.IsThisDeviceBelongsToCurrentUser(guid);
        return guid;
    }
}