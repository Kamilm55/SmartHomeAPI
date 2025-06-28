using Smart_Home_IoT_Device_Management_API.Application.Services;


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Domain.Enum;
using LoginRequest = Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.LoginRequest;

namespace Smart_Home_IoT_Device_Management_API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        // Inject the service layer
        public UsersController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        

        // GET: api/v1/users -> SuperAdmin/Admin-in device-lari hansi userlara aiddirse butun listler
        // SuperAdmin -> all users
        // Admin -> Yalniz eyni evde olan userlar
        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.SuperAdmin)},{nameof(Role.Admin)}")]
        public async Task<ActionResult<ApiResponse<List<UserResponse>>>> GetAllUsers()
        {
            List<UserResponse> userResponse = await _userService.GetAllUsersBelongToCurrentUser();

            return ApiResponse<List<UserResponse>>.Ok(userResponse);
        }
        
        // GET: api/v1/users/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserById(string id)
        {
            UserResponse? userResponse = await _userService.GetUserByIdAsync(id);

            return ApiResponse<UserResponse>.Ok(userResponse);
        }

        // POST: api/v1/users
        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserResponse>>> CreateUser([FromBody] UserCreateRequest dto)
        {
            UserResponse? createdUser = await _userService.CreateUserAsync(dto);

            return ApiResponse<UserResponse>.Created(createdUser, nameof(GetUserById) + new { id = createdUser.Id });
        }
        
        //
        [HttpPatch("{id}/assign-admin")]
        [Authorize(Roles = nameof(Role.SuperAdmin))] // Only SuperAdmin allowed
        public async Task<ActionResult<ApiResponse<string>>> AssignAdminRole(string id)
        {
            await _userService.AssignAdminRoleAsync(id);
            
            return ApiResponse<string>.NoContent($"User with id:{id} is assigned to admin role successfully");
        }

        [HttpPatch("{id}/assign-userRole")]
        [Authorize(Roles = nameof(Role.Admin))]  // Only Admin can access
        public async Task<ActionResult<ApiResponse<string>>> AssignUserRole(string id, [FromQuery] UserAccessLevel accessLevel)
        {
            // Your logic here, e.g. update user's access level by id
            // Example:
             await _userService.AssignUserRoleAsync(id, accessLevel);

             return ApiResponse<string>.NoContent($"User with id:{id} is assigned to {accessLevel} role successfully");

        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login(LoginRequest request)
        {
            // Validate user credentials (e.g., check username and password)
            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            
            
            var token = await _jwtService.GenerateToken(user);
            return ApiResponse<string>.Ok(token, "Login successful");
        }

        [Authorize] 
        [HttpGet("me")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> GetCurrentUser()
        {
            UserResponse? userResponse = await _userService.getCurrentUser();
            return ApiResponse<UserResponse>.Ok(userResponse,"Current user info:");
        }
    }
}
