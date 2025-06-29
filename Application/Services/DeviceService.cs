using Microsoft.AspNetCore.Identity;
using Smart_Home_IoT_Device_Management_API.Application.Mappers;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Device;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses.Device;
using Smart_Home_IoT_Device_Management_API.Common.Exceptions;
using Smart_Home_IoT_Device_Management_API.Common.Utils;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Domain.Entities.Owned;
using Smart_Home_IoT_Device_Management_API.Domain.Enum;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IDeviceCategoryRepository _deviceCategoryRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository  _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IHelperService _helperService;
    private readonly IMapper _mapper;

    public DeviceService(
        IDeviceRepository deviceRepository,
        IDeviceCategoryRepository deviceCategoryRepository,
        ILocationRepository locationRepository,
        IUnitOfWork unitOfWork, IUserRepository userRepository, UserManager<User> userManager, IHelperService helperService, IMapper mapper)
    {
        _deviceRepository = deviceRepository;
        _deviceCategoryRepository = deviceCategoryRepository;
        _locationRepository = locationRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _userManager = userManager;
        _helperService = helperService;
        _mapper = mapper;
    }
    
    public async Task<List<DeviceResponse>> GetAllDevicesAsync()
    {
        var currentUser = await _helperService.getCurrentUserFromToken();
        var devices = await _deviceRepository.GetAllByUserIdWithCategoryAndLocationAsync(currentUser.Id);
        return devices.Select(DeviceMapper.ToDeviceResponse).ToList();
    }

    public async Task<DeviceResponse> GetDeviceByIdAsync(string id)
    {
        var deviceId = ParseGuid(id, nameof(Device));
        var device = await GetDeviceOrThrowAsync(deviceId, includeRelations: true);
        await _helperService.IsThisDeviceBelongsToCurrentUser(deviceId);
        return DeviceMapper.ToDeviceResponse(device);
    }

    public async Task<DeviceResponse> CreateDeviceAsync(DeviceCreateRequest request)
    {
        var deviceCategoryId = GuidParser.Parse(request.DeviceCategoryId, nameof(DeviceCategory));
        var locationId = GuidParser.Parse(request.LocationId, nameof(Location));

        var category = await _deviceCategoryRepository.GetByIdAsync(deviceCategoryId)
                       ?? throw new NotFoundException(nameof(DeviceCategory), deviceCategoryId);

        var location = await _locationRepository.GetByIdAsync(locationId)
                       ?? throw new NotFoundException(nameof(Location), locationId);

        var device = _mapper.ToDevice(request, location, category);

        // Add every device to superAdmin
        var superAdmin = (await _userManager.GetUsersInRoleAsync(nameof(Role.SuperAdmin))).Single();
        superAdmin.Devices.Add(device);
        device.Users.Add(superAdmin);
        
        var createdDevice = await _deviceRepository.SaveDeviceAndReturnLatest(device);

        return DeviceMapper.ToDeviceResponse(createdDevice);
    }

    public async Task<DeviceResponse> UpdateDeviceAsync(string id, DeviceUpdateRequest request)
    {
        var deviceId = ParseGuid(id, nameof(Device));
        var deviceById = await GetDeviceOrThrowAsync(deviceId, includeRelations: true);

        await _helperService.IsThisDeviceBelongsToCurrentUser(deviceId);
        
        if (request.DeviceCategoryId != null)
        {
            var deviceCategoryId = GuidParser.Parse(request.DeviceCategoryId, nameof(DeviceCategory));
            await _deviceCategoryRepository.ExistsByIdAsync(deviceCategoryId);
        }

        if (request.LocationId != null)
        {
            var locationId = GuidParser.Parse(request.LocationId, nameof(Location));
            await _locationRepository.ExistsByIdAsync(locationId);
        }


        var device = _mapper.ToDevice(request,deviceById);

        var savedDevice = await _deviceRepository.SaveDeviceAndReturnLatest(device);
        return DeviceMapper.ToDeviceResponse(savedDevice);
    }

    public async Task DeleteDeviceAsync(string id)
    {
        var deviceId = ParseGuid(id, nameof(Device));
        var device = await GetDeviceOrThrowAsync(deviceId, includeRelations: true);
        await _helperService.IsThisDeviceBelongsToCurrentUser(deviceId);

        await _deviceRepository.Delete(device);
        await _deviceRepository.SaveDeviceAndReturnLatest(device);
    }

    public async Task AssignDeviceToAdminRoleAsync(string id, string userId)
    {
        var device = await GetDeviceOrThrowAsync(ParseGuid(id, nameof(Device)));
        var user = await GetUserOrThrowAsync(ParseGuid(userId, nameof(User)));

        // If current user has admin role
        await ValidateUserRoleAsync(user, nameof(Role.Admin));

        // Assign device to user
        device.Users.Add(user);
        user.Devices.Add(device);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AssignDeviceToUserRoleAsync(string id, string userId)
    {
        var device = await GetDeviceOrThrowAsync(ParseGuid(id, nameof(Device)));
        var user = await GetUserOrThrowAsync(ParseGuid(userId, nameof(User)));

        await ValidateUserRoleAsync(user, nameof(Role.UserReadOnly), nameof(Role.UserReadWrite));

        await _helperService.IsThisDeviceBelongsToCurrentUser(device.Id);
        device.Users.Add(user);
        user.Devices.Add(device);

        await _unitOfWork.SaveChangesAsync();
    }
    
    // Helpers
    private static Guid ParseGuid(string id, string name) => GuidParser.Parse(id, name);

    private async Task<Device> GetDeviceOrThrowAsync(Guid id, bool includeRelations = false)
    {
        var device = includeRelations
            ? await _deviceRepository.GetByIdWithCategoryAndLocationAsync(id)
            : await _deviceRepository.GetByIdAsync(id);

        return device ?? throw new NotFoundException(nameof(Device), id);
    }

    private async Task<User> GetUserOrThrowAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id)
               ?? throw new NotFoundException(nameof(User), id);
    }

    private async Task ValidateUserRoleAsync(User user, params string[] validRoles)
    {
        foreach (var role in validRoles)
        {
            if (await _userManager.IsInRoleAsync(user, role))
                return;
        }

        throw new InvalidOperationException($"User with ID: {user.Id} does not have a valid role.");
    }

}