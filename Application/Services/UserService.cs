using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Smart_Home_IoT_Device_Management_API.Application.Mappers;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Common.Exceptions;
using Smart_Home_IoT_Device_Management_API.Common.Utils;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
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

    public UserService(IUserRepository userRepository, ILogger<UserService> logger, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
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
            CreatedAt = DateTime.UtcNow
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
        
        return isValidPassword ? user : throw new InvalidEmailOrPasswordException("Invalid email or password");
    }

    public string getHashedPwd(string pwd)
    {
      return  _passwordHasher.Hash(pwd);
    }

    public async Task<UserResponse?> getCurrentUser()
    {
        string? emailFromToken = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) 
                                 ??  throw new ArgumentNullException("Email from token is null");
        
        User? user = await _userRepository.GetByEmailAsync(emailFromToken) 
                                 ?? throw new NotFoundException($"User not fount with email:{emailFromToken}");
        
        return UserMapper.ToResponse(user);
    }
}