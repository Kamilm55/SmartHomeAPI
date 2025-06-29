using System.ComponentModel.DataAnnotations;

namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Location;

public class LocationCreateRequest
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = null!;  // Required

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }   // Optional

    [Range(-5, 100, ErrorMessage = "FloorNumber must be between -5 and 100.")]
    public int FloorNumber { get; set; } = 0;  // Non-nullable with default 0

    [Range(1, int.MaxValue, ErrorMessage = "RoomId must be a positive number.")]
    public int RoomId { get; set; } = 0;       // Non-nullable with default 0
}