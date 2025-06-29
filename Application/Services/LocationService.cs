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
        IMapper mapper, IHelperService helperService)
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
        var locationId = GuidParser.Parse(nameof(Location), id);

        var location = await _locationRepository.GetByIdWithDevicesAndDeviceUsersAsync(locationId);

        if (location is null)
            throw new NotFoundException(nameof(Location), locationId);

        await IsThisLocationBelongsToCurrentUser(location);

        return location.Devices.Select(_mapper.ToDeviceInLocationResponse).ToList();
    }

    private async Task IsThisLocationBelongsToCurrentUser(Location location)
    {
        var currentUser = await _helperService.getCurrentUserFromToken();
        var allUsersForThisLocation = location.Devices.SelectMany(d => d.Users);
        
        if (allUsersForThisLocation.Any(u => u.Id != currentUser.Id))
        {
            throw new InvalidOperationException($"This location of device with id:{location.Id} does not belong to you");
        }
        
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