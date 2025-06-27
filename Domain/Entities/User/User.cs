using Microsoft.AspNetCore.Identity;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string? FullName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    // Many-to-Many with Devices
    public ICollection<Device> Devices { get; set; } = new HashSet<Device>();

    public override string ToString()
    {
        return $"User {{ " +
               $"Id = {Id}, " +
               $"UserName = {UserName}, " +
               $"Email = {Email}, " +
               $"FullName = {FullName ?? "null"}, " +
               $"CreatedAt = {CreatedAt}, " +
               $"UpdatedAt = {UpdatedAt?.ToString() ?? "null"}, " +
               $"LastLoginAt = {LastLoginAt?.ToString() ?? "null"}, " +
               $"Devices.Count = {Devices?.Count ?? 0} " +
               $"}}";
    }
}