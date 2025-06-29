using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Smart_Home_IoT_Device_Management_API.Common.Exceptions;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public class HelperService : IHelperService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<User> _userManager;

    public HelperService(IUserRepository userRepository, ILogger<UserService> logger, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
    {
        _userRepository = userRepository;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<User> getCurrentUserFromToken()
    {
        string? emailFromToken = _httpContextAccessor.HttpContext?.User?
                                     .FindFirst(ClaimTypes.Email)?.Value
                                 ?? throw new ArgumentNullException("Email from token is null");

        User? user = await _userRepository.GetByEmailAsync(emailFromToken) 
                     ?? throw new NotFoundException($"User not fount with email:{emailFromToken}");
        return user;
    }

    public async Task IsThisDeviceBelongsToCurrentUser(Guid deviceId)
    {
        var currentUser = await getCurrentUserFromToken();
        if (currentUser.Devices.All(d => d.Id != deviceId))
        {
            throw new InvalidOperationException($"Device with id:{currentUser.Id} does not belong to you. You cannot read another's device details!");
        }
    }
}