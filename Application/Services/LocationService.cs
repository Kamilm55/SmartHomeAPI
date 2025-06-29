using Smart_Home_IoT_Device_Management_API.Application.Mappers;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Location;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Common.Exceptions;
using Smart_Home_IoT_Device_Management_API.Common.Utils;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHelperService _helperService;

    public LocationService(
        ILocationRepository locationRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHelperService helperService)
    {
        _locationRepository = locationRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _helperService = helperService;
    }

    public async Task<List<LocationResponse>> GetAllAsync()
    {
        var currentUser = await _helperService.getCurrentUserFromToken();
        var locations = await _locationRepository.GetAllByUserDevicesIdAsync(currentUser.Devices);
        return locations.Select(_mapper.ToLocationResponse).ToList();
    }

    public async Task<List<DeviceInLocationResponse>> GetDevicesByLocationIdAsync(string id)
    {
        var location = await GetLocationWithValidationAsync(id);
        return location.Devices.Select(_mapper.ToDeviceInLocationResponse).ToList();
    }

    public async Task<LocationResponse> CreateAsync(LocationCreateRequest request)
    {
        var location = _mapper.ToLocation(request);

        await _locationRepository.AddAsync(location);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToLocationResponse(location);
    }

    // Helpers
    private async Task<Location> GetLocationWithValidationAsync(string id)
    {
        var locationId = GuidParser.Parse(nameof(Location), id);

        var location = await _locationRepository.GetByIdWithDevicesAndDeviceUsersAsync(locationId)
                       ?? throw new NotFoundException(nameof(Location), locationId);

        await ValidateLocationOwnershipAsync(location);
        return location;
    }

    private async Task ValidateLocationOwnershipAsync(Location location)
    {
        var currentUser = await _helperService.getCurrentUserFromToken();

        var allUsersId = location.Devices.SelectMany(d => d.Users).Select(u => u.Id).Distinct();

        if (!allUsersId.Contains(currentUser.Id))
        {
            throw new InvalidOperationException($"This location with ID: {location.Id} does not belong to you.");
        }
    }
}
