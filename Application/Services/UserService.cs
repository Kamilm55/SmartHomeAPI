using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Smart_Home_IoT_Device_Management_API.Application.Mappers;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Common.Exceptions;
using Smart_Home_IoT_Device_Management_API.Common.Utils;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
using Smart_Home_IoT_Device_Management_API.Domain.Enum;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Repositories.Interfaces;
using Smart_Home_IoT_Device_Management_API.Infrastructure.Security;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly IHelperService _helperService;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork, UserManager<User> userManager, IHelperService helperService)
    {
        _userRepository = userRepository;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _helperService = helperService;
    }

    public async Task<UserResponse> GetUserByIdAsync(string idStr)
    {
        var id = ParseUserId(idStr);
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("User", id);

        return UserMapper.ToResponse(user);
    }

    public async Task<UserResponse?> CreateUserAsync(UserCreateRequest request)
    {
        if (await _userRepository.GetByEmailAsync(request.Email) is not null)
            throw new UserAlreadyExistException("email", request.Email);

        if (await _userRepository.GetByUsernameAsync(request.Username) is not null)
            throw new UserAlreadyExistException("username", request.Username);

        var hashedPassword = _passwordHasher.Hash(request.Password);

        var user = UserMapper.ToUser(request, hashedPassword);

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return UserMapper.ToResponse(user);
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email)
            ?? throw new InvalidEmailOrPasswordException("Invalid email");

        if (!_passwordHasher.Verify(password, user.PasswordHash))
            throw new InvalidEmailOrPasswordException("Invalid password");

        user.LastLoginAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();

        return user;
    }

    public string GetHashedPassword(string password) => _passwordHasher.Hash(password);

    public async Task<UserResponse?> GetCurrentUserAsync()
    {
        var user = await _helperService.getCurrentUserFromToken();
        return UserMapper.ToResponse(user);
    }

    public async Task<List<UserResponse>> GetAllUsersBelongToCurrentUserAsync()
    {
        var user = await _helperService.getCurrentUserFromToken();
        var users = await _userRepository.GetByDevicesAsync(user.Devices);
        return users.Select(UserMapper.ToResponse).ToList();
    }

    public async Task AssignAdminRoleAsync(string id)
    {
        var guid = ParseUserId(id);
        var user = await FindUserByIdOrThrowAsync(guid);

        await RemoveAllRolesExceptAsync(user, nameof(Role.SuperAdmin));
        await AddRoleIfNotExistsAsync(user, nameof(Role.Admin));
    }

    public async Task AssignUserRoleAsync(string id, UserAccessLevel accessLevel)
    {
        var guid = ParseUserId(id);
        var user = await FindUserByIdOrThrowAsync(guid);

        await RemoveAllRolesExceptAsync(user, nameof(Role.SuperAdmin), nameof(Role.Admin));
        var roleName = accessLevel.ToString();
        await AddRoleIfNotExistsAsync(user, roleName);
    }
    
    // Helpers
    private static Guid ParseUserId(string id) =>
        GuidParser.Parse(id, nameof(User));

    private async Task<User> FindUserByIdOrThrowAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString())
                   ?? throw new NotFoundException($"User not found with id: {id}");
        return user;
    }

    private async Task RemoveAllRolesExceptAsync(User user, params string[] excludedRoles)
    {
        var currentRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in currentRoles)
        {
            if (excludedRoles.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Operation not allowed: user has protected role '{role}'");
            
            await _userManager.RemoveFromRoleAsync(user, role);
        }
    }

    private async Task AddRoleIfNotExistsAsync(User user, string role)
    {
        if (!await _userManager.IsInRoleAsync(user, role))
            await _userManager.AddToRoleAsync(user, role);
    }
}