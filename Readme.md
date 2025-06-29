# 📡 Smart Home IoT Device Management API

The **Smart Home IoT Device Management API** is a secure and scalable RESTful backend designed to manage smart home devices, users, locations, and sensor data with strict role-based access control and clear ownership rules.

---

## 🔧 Core Features

### 👥 User & Role Management
- Create, authenticate, and authorize users with hierarchical roles:
    - `SuperAdmin`
    - `Admin`
    - `UserReadOnly` / `UserReadWrite`
- **SuperAdmins** manage Admins.
- **Admins** manage Users.
- Role-based permissions control what each user can access or modify.

### 📱 Device Management
- Full CRUD operations on IoT devices.
- Devices are always tied to users based on their role.
- SuperAdmins can assign devices to Admins.
- Admins can assign devices to Users.
- Each user retrieves and manages only their authorized devices.

### 📍 Location Management
- Devices are organized by physical locations.
- Locations can be created by SuperAdmins or Admins.
- Retrieve all devices per location for structured management.

### 📊 Sensor Data Handling
- Add and retrieve device sensor readings (temperature, humidity, battery, etc.).
- Supports real-time monitoring and history tracking.

### 📈 Analytics
- Energy usage reports
- Device status summaries
- Location-specific usage metrics
- Device health insights (e.g., signal strength, battery level)

---

## 🔐 Role-Based Access Notes

- All data retrieval methods return only data **belonging to the authenticated user**.
- By **default**, all newly created devices are assigned to the **SuperAdmin**.

### 🔑 SuperAdmin
- ✅ Can view all devices and data
- ✅ Can assign Admin roles
- ✅ Can assign devices to Admins
- ❌ Cannot assign User roles

### 🧑‍💼 Admin
- ✅ Can assign User roles (`UserReadOnly`, `UserReadWrite`)
- ✅ Can assign devices to Users

### 👤 User
- ✅ Can only access their own assigned devices
- `UserReadOnly`: View-only
- `UserReadWrite`: View and update access

---

## 🧱 Device Design 

To ensure **scalability**, **data normalization**, and to **reduce redundancy**, the device model is broken into three layers:

* DeviceCategory: Describes reusable/shared properties of a type of IoT device (e.g., "Smart Bulb", "Thermostat").
* Device: A physical instance of a device in the system, installed somewhere and uniquely identified.
* DeviceTypeGroup: A higher-level optional classification (e.g., Lighting, Climate, Security)

### 🧩 DeviceCategory
- Describes shared, reusable properties of a device type (e.g., "Smart Bulb", "Thermostat").
- Includes common attributes like supported features and configuration templates.

### 🛰️ Device
- Represents a unique physical IoT device installed in a location.
- Linked to a `DeviceCategory`.

### 🗂️ DeviceTypeGroup (optional)
- Groups related categories under a higher-level type (e.g., Lighting, Climate, Security).

### 💡 Why This Design?
- Many physical devices share identical configurations.
- Without abstraction, you'd duplicate some repeated properties per device.
- This structure:
    - ✅ Avoids data duplication
    - ✅ Improves consistency
    - ✅ Reduces storage usage
    - ✅ Simplifies the creation of new device types
    - ✅ Matches real-world modeling where devices of the same model share behavior but are installed independently

---

## 🛠️ How It Works

- 🔐 **JWT-based authentication** secures all endpoints.
- 🛡️ **Role-based authorization** with `[Authorize(Roles = ...)]` enforces access control.
- 🔄 Each user interacts **only with their own** devices, locations, and data.
- 👑 SuperAdmin has **central visibility** and authority.
- 📊 Sensor and analytics endpoints offer meaningful insights into performance and usage.

---

This API provides a clean, secure, and extensible platform for managing smart home IoT devices and users in multi-role environments.

---

## ⚙️ Setup Instructions

### 📁 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- SQL Server or PostgreSQL (your DB)


### 🧪 Local Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/smart-home-iot-api.git
   cd Smart_Home_IoT_Device_Management_API
   ```

2. **Configure your environment**

   Create a `appsettings.Development.json` (or use `appsettings.json`) and add the following:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Your-DB-Connection-String"
     },
     "JwtSettings": {
       "Key": "your_secret_key",
       "Issuer": "your_app_name",
       "Audience": "your_app_users",
       "ExpiresInMinutes": 60
     }
   }
   ```

3. **Run database migrations** (if using EF Core)

   ```bash
   dotnet ef database update
   ```

4. **Run the application**

   ```bash
   dotnet run
   ```

5. **Access Swagger UI (API Docs)**

   ```
   http://localhost:{your-port}/swagger/index.html
   ```
