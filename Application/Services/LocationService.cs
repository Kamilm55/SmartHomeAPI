using Smart_Home_IoT_Device_Management_API.Application.Mappers;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Location;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Common.Exceptions;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper; 

    public LocationService(
        ILocationRepository locationRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _locationRepository = locationRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<LocationResponse>> GetAllAsync()
    {
        var locations = await _locationRepository.GetAllAsync();
        return locations.Select(_mapper.ToLocationResponse).ToList();
    }

    public async Task<List<DeviceInLocationResponse>> GetDevicesByLocationIdAsync(string id)
    {
        if (!Guid.TryParse(id, out var locationId))
            throw new InvalidGuidException(nameof(Location),id);

        var location = await _locationRepository.GetByIdWithDevicesAsync(locationId);

        if (location is null)
            throw new NotFoundException(nameof(Location), locationId);

        return location.Devices.Select(_mapper.ToDeviceInLocationResponse).ToList();
    }

    public async Task<LocationResponse> CreateAsync(LocationCreateRequest request)
    {
        var location = new Location
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            FloorNumber = request.FloorNumber,
            RoomId = request.RoomId
        };

        await _locationRepository.AddAsync(location);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToLocationResponse(location);
    }
}