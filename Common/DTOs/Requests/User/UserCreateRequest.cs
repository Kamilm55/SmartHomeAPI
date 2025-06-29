using System.ComponentModel.DataAnnotations;

namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests;

public class UserCreateRequest
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = null!; // Will be hashed inside service

    // Optional
    [StringLength(100, ErrorMessage = "Full name must not exceed 100 characters.")]
    public string? FullName { get; set; }
}