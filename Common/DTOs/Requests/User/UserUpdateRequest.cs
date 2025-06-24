namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests;

public class UserUpdateRequest
{
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? Password { get; set; }  // optional password change
}