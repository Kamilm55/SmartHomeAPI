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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<User> _userManager;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
    {
        _userRepository = userRepository;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }


    public async Task<UserResponse> GetUserByIdAsync(string idStr)
    {
        Guid id = GuidParser.Parse(idStr,nameof(User));
        User? user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User",id);

        return UserMapper.ToResponse(user);
    }

    public async Task<UserResponse?> CreateUserAsync(UserCreateRequest request)
    {
        // 1. Is Exist by email or username
        User? existingUserWithEmail = await _userRepository
            .GetByEmailAsync(request.Email);

        if (existingUserWithEmail is not null)
        {
            throw new UserAlreadyExistException("email", request.Email);
        }
        
        User? existingUserWithUsername = await _userRepository
            .GetByUsernameAsync(request.Username);

        if (existingUserWithUsername is not null)
        {
            throw new UserAlreadyExistException("username", request.Username);
        }

        // 2. Hash the password
        var hashedPassword = _passwordHasher.Hash(request.Password);

        // 3. Create User entity
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.Username,
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        // 4. Save to database
        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // 5. Return the UserResponse DTO
        return UserMapper.ToResponse(user);
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        User? user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            throw new InvalidEmailOrPasswordException("Invalid email or password");
        }

        bool isValidPassword = _passwordHasher.Verify(password, user.PasswordHash);
        
        user.LastLoginAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        
        return isValidPassword ? user : throw new InvalidEmailOrPasswordException("Invalid email or password");
    }

    public string getHashedPwd(string pwd)
    {
      return  _passwordHasher.Hash(pwd);
    }

    public async Task<UserResponse?> getCurrentUser()
    {
        var user = await getCurrentUserFromToken();

        return UserMapper.ToResponse(user);
    }

    private async Task<User> getCurrentUserFromToken()
    {
        string? emailFromToken = _httpContextAccessor.HttpContext?.User?
                                     .FindFirst(ClaimTypes.Email)?.Value
                                 ?? throw new ArgumentNullException("Email from token is null");

        User? user = await _userRepository.GetByEmailAsync(emailFromToken) 
                     ?? throw new NotFoundException($"User not fount with email:{emailFromToken}");
        return user;
    }

    public async Task<List<UserResponse>> GetAllUsersBelongToCurrentUser()
    {
        User? user = await getCurrentUserFromToken();
        List<User> users = await _userRepository.getByDevicesAsync(user.Devices);
        
        return users.Select(UserMapper.ToResponse).ToList();
    }

    public async Task AssignAdminRoleAsync(string id)
    {
        Guid guid = GuidParser.Parse(id,nameof(User));
        var user = await _userManager.FindByIdAsync(guid.ToString()) 
                   ?? throw new NotFoundException($"User not fount with id:{guid.ToString()}");
      
        
        // Get current roles
        var roles = await _userManager.GetRolesAsync(user);

        // Remove all roles except SuperAdmin
        foreach (var role in roles)
        {
            if (role.Equals(nameof(Role.SuperAdmin), StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"You cannot add admin role to user has {role} role"); 
            }
            await _userManager.RemoveFromRoleAsync(user, role);
        }

        // Add Admin role if not already in it
        if (!await _userManager.IsInRoleAsync(user, nameof(Role.Admin)))
        {
            await _userManager.AddToRoleAsync(user, nameof(Role.Admin));
        }

        
    }

    public async Task AssignUserRoleAsync(string id, UserAccessLevel accessLevel)
    {
        Guid guid = GuidParser.Parse(id,nameof(User));
        var user = await _userManager.FindByIdAsync(guid.ToString()) 
                   ?? throw new NotFoundException($"User not fount with id:{guid.ToString()}");

        
        // Get current roles
        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            if (role.Equals(nameof(Role.SuperAdmin), StringComparison.OrdinalIgnoreCase)
                ||  role.Equals(nameof(Role.Admin), StringComparison.OrdinalIgnoreCase)
                )
            {
                throw new InvalidOperationException($"You cannot add {accessLevel} role to user has {role} role"); 
            }
            await _userManager.RemoveFromRoleAsync(user, role);
        }

        // Add User role if not already in it
        var roleName = accessLevel.ToString();

        if (!await _userManager.IsInRoleAsync(user, roleName))
        {
            // Since roleName matches the Role enum names, add role directly
            await _userManager.AddToRoleAsync(user, roleName);
        }

        
    }
}