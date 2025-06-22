namespace Smart_Home_IoT_Device_Management_API.Domain.Enum;

public enum PowerSource
{
    Battery = 1, // Standard replaceable or rechargeable battery
    Rechargeable = 2, // Built-in rechargeable (e.g., lithium-ion)
    Mains = 3, // Connected to wall power
    Solar = 4, // Powered by solar panel
    PoE = 5, // Power over Ethernet
    USB = 6, // USB-powered (e.g., micro-USB, USB-C)
    WirelessCharging = 7, // Inductive/wireless power
    Hybrid = 8, // Combination (e.g., solar + battery)
    FuelCell = 9, // Rare, for industrial sensors
    Kinetic = 10 // Powered by motion (e.g., self-winding switches)
}