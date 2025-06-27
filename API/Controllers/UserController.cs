using Smart_Home_IoT_Device_Management_API.Application.Services;


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using LoginRequest = Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.LoginRequest;

namespace Smart_Home_IoT_Device_Management_API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        // Inject the service layer
        public UserController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        // GET: api/v1/user/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserById(string id)
        {
            UserResponse? userResponse = await _userService.GetUserByIdAsync(id);

            Console.WriteLine(userResponse);
            return ApiResponse<UserResponse>.Ok(userResponse);
        }

        // POST: api/v1/user
        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserResponse>>> CreateUser([FromBody] UserCreateRequest dto)
        {
            UserResponse? createdUser = await _userService.CreateUserAsync(dto);

            return ApiResponse<UserResponse>.Created(createdUser, nameof(GetUserById) + new { id = createdUser.Id });
        }
        
        //

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login(LoginRequest request)
        {
            // Validate user credentials (e.g., check username and password)
            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            
            
            var token = _jwtService.GenerateToken(user);
            return ApiResponse<string>.Ok(token, "Login successful");
        }

        [HttpGet("me")]
        [Authorize ]
        public async Task<ActionResult<ApiResponse<UserResponse>>> GetCurrentUser()
        {
            UserResponse? userResponse = await _userService.getCurrentUser();
            return ApiResponse<UserResponse>.Ok(userResponse,"Current user info:");
        }
    }
}
