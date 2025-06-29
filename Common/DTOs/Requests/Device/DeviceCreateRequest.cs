using System;
using System.ComponentModel.DataAnnotations;

namespace Smart_Home_IoT_Device_Management_API.Common.DTOs.Requests.Device;

public class DeviceCreateRequest
{
    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "SerialNumber is required.")]
    [StringLength(100, ErrorMessage = "SerialNumber cannot exceed 100 characters.")]
    public string SerialNumber { get; set; } = null!;

    [Required(ErrorMessage = "MACAddress is required.")]
    /*[RegularExpression("^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$",
        ErrorMessage = "MACAddress must be in the format XX:XX:XX:XX:XX:XX.")]*/
    public string MACAddress { get; set; } = null!;

    // Numeric fields - non-nullable with default 0, no Required attribute
    [Range(0, 100, ErrorMessage = "Brightness must be between 0 and 100.")]
    public int Brightness { get; set; } = 0;

    [Range(0, 100, ErrorMessage = "Volume must be between 0 and 100.")]
    public int Volume { get; set; } = 0;

    [Range(-150, 150, ErrorMessage = "TemperatureThreshold must be between -150 and 150.")]
    public int TemperatureThreshold { get; set; } = 0;

    public bool AutoShutdown { get; set; } = false;

    [Range(0, 40, ErrorMessage = "MotionSensitivity must be between 0 and 40.")]
    public int MotionSensitivity { get; set; } = 0;

    [Range(1, 36000, ErrorMessage = "UpdateIntervalSeconds must be between 1 and 36000.")]
    public int UpdateIntervalSeconds { get; set; } = 0;

    // Required Guids
    [Required(ErrorMessage = "DeviceCategoryId is required.")]
    public string DeviceCategoryId { get; set; } = null!;

    [Required(ErrorMessage = "LocationId is required.")]
    public string LocationId { get; set; } = null!;
}