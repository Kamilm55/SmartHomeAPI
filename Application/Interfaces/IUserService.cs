using Microsoft.AspNetCore.Mvc;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Application.Services;

public interface IUserService
{
    Task<UserResponse> GetUserByIdAsync(string id);
    Task<UserResponse?> CreateUserAsync(UserCreateRequest userCreateRequest);
    Task<User?> AuthenticateAsync(string email, string password);

    string getHashedPwd(string pwd);
    Task<UserResponse?> getCurrentUser();
    Task<List<UserResponse>> GetAllUsersBelongToCurrentUser();
    Task AssignAdminRoleAsync(string id);
    Task AssignUserRoleAsync(string id, UserAccessLevel accessLevel);
}