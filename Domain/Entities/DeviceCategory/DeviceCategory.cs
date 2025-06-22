using Smart_Home_IoT_Device_Management_API.Domain.Enum;

namespace Smart_Home_IoT_Device_Management_API.Domain.Entities;

public class DeviceCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!; // e.g., "Thermostat", "Light", "Security Camera"
    public string Manufacturer { get; set; } = null!;
    public string FirmwareVersion { get; set; } = null!;
    
    public PowerSource PowerSource { get; set; } //  (e.g., Battery, Mains, Solar)
    public bool RequiresInternet { get; set; } = false;

    public string? Description { get; set; }
    public string? CommunicationProtocol { get; set; }// (e.g., Wi-Fi, Zigbee, Bluetooth)
    public DeviceTypeGroup? DeviceType { get; set; } = DeviceTypeGroup.Not_Defined;
    
    // Relationships
    // One to Many
    public ICollection<Device> Devices { get; set; } = new HashSet<Device>();

}
