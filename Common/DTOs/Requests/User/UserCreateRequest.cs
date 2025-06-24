namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests;

public class UserCreateRequest
{
    // Required fields 
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    
    // Hash inside service
    public string Password { get; set; } = null!;

    // Optional
    public string? FullName { get; set; }
    
}