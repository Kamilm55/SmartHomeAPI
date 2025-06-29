using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Home_IoT_Device_Management_API.Application.Services;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Domain.Enum;
using LoginRequest = Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.LoginRequest;

namespace Smart_Home_IoT_Device_Management_API.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UsersController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Retrieves all users related to the current SuperAdmin/Admin.
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns list of users</response>
        /// <response code="401">Unauthorized - Token is missing or invalid</response>
        /// <response code="403">Forbidden - User does not have required role</response>
        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.SuperAdmin)},{nameof(Role.Admin)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<List<UserResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<UserResponse>>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersBelongToCurrentUserAsync();
            return ApiResponse<List<UserResponse>>.Ok(users);
        }

        /// <summary>
        /// Retrieves a user by their ID. Only SuperAdmin can use.
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <returns>User details</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="404">User not found</response>
        [HttpGet("{id}")]
        [Authorize(Roles = nameof(Role.SuperAdmin))]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return ApiResponse<UserResponse>.Ok(user);
        }

        /// <summary>
        /// Creates a new user. Register endpoint for all.
        /// </summary>
        /// <param name="userCreateRequest">User creation data</param>
        /// <returns>The created user</returns>
        /// <response code="201">User created successfully</response>
        /// <response code="400">Invalid input</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<UserResponse>>> CreateUser([FromBody] UserCreateRequest userCreateRequest)
        {
            var createdUser = await _userService.CreateUserAsync(userCreateRequest);
            return ApiResponse<UserResponse>.Created(createdUser, nameof(GetUserById) + new { id = createdUser.Id });
        }

        /// <summary>
        /// Assigns the Admin role to a user. Only SuperAdmins can perform this.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Admin role assigned</response>
        /// <response code="404">User not found</response>
        /// <response code="400">You cannot add admin role to user has SuperAdmin role</response>
        [HttpPatch("{id}/assign-admin")]
        [Authorize(Roles = nameof(Role.SuperAdmin))]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<string>>> AssignAdminRole(string id)
        {
            await _userService.AssignAdminRoleAsync(id);
            return ApiResponse<string>.NoContent($"User with id:{id} is assigned to admin role successfully");
        }

        /// <summary>
        /// Assigns a user role (UserReadOnly/UserReadWrite). Only Admins can perform this.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="accessLevel">Access level to assign</param>
        /// <returns>No content</returns>
        /// <response code="204">Role assigned</response>
        /// <response code="404">User not found</response>
        /// <response code="400">You cannot add User role to user has Admin role</response>
        [HttpPatch("{id}/assign-userRole")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<string>>> AssignUserRole(string id, [FromQuery] UserAccessLevel accessLevel)
        {
            await _userService.AssignUserRoleAsync(id, accessLevel);
            return ApiResponse<string>.NoContent($"User with id:{id} is assigned to {accessLevel} role successfully");
        }

        /// <summary>
        /// Authenticates the user and returns a JWT token.
        /// </summary>
        /// <param name="request">Login credentials</param>
        /// <returns>JWT token</returns>
        /// <response code="200">Login successful</response>
        /// <response code="400">Invalid credentials</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<string>>> Login(LoginRequest request)
        {
            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            var token = await _jwtService.GenerateToken(user);
            return ApiResponse<string>.Ok(token, "Login successful");
        }

        /// <summary>
        /// Returns the authenticated user's details.
        /// </summary>
        /// <returns>Current user info</returns>
        /// <response code="200">Returns the authenticated user</response>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<UserResponse>>> GetCurrentUser()
        {
            var user = await _userService.GetCurrentUserAsync();
            return ApiResponse<UserResponse>.Ok(user, "Current user info:");
        }
    }
}
