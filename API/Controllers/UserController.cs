using Smart_Home_IoT_Device_Management_API.Application.Services;


using Microsoft.AspNetCore.Mvc;
using Smart_Home_IoT_Device_Management_API.Application.Services;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using Smart_Home_IoT_Device_Management_API.Common;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests;
using Smart_Home_IoT_Device_Management_API.Common.DTOs.Responses;
using Smart_Home_IoT_Device_Management_API.Common.Exceptions;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;
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
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/v1/user/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserById(string id)
        {
            UserResponse? userResponse = await _userService.GetUserByIdAsync(id);

            return ApiResponse<UserResponse>.Ok(userResponse);
        }

        // POST: api/v1/user
        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserResponse>>> CreateUser([FromBody] UserCreateRequest dto)
        {
            UserResponse? createdUser = await _userService.CreateUserAsync(dto);

            return ApiResponse<UserResponse>.Created(createdUser, nameof(GetUserById) + new { id = createdUser.Id });
        }

        [HttpPost("/login")]
        public async Task<ActionResult<ApiResponse<string>>> Login(LoginRequest request)
        {
            // Validate user credentials (e.g., check username and password)
            var user = await _userService.AuthenticateAsync(request.Email, request.Password);

            if (user == null)
            {
                throw new InvalidEmailOrPasswordException("Invalid email or password");
            }

            var token = _jwtService.GenerateToken(user);
            return ApiResponse<string>.Ok(token, "Login successful");
        }

        /*[HttpPost]
        public string Login(string pwd)
        {
           return  _userService.getHashedPwd(pwd);
        }*/
    }
}
