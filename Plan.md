# Smart Home IoT Device Management API - Intern Project

## Project Overview
You will build a RESTful API that manages smart home devices like thermostats, lights, motion sensors, and smart plugs. This API will track device data and provide analytics on usage patterns.

## Learning Goals
- Entity Framework Core with relationships
- RESTful API design principles
- Data validation and error handling
- LINQ queries and data filtering
- Working with DateTime and data aggregation
- API testing and documentation

## Project Setup

### Prerequisites
- Visual Studio 2022, VS Code, or JetBrains Rider
- .NET 8 SDK
- PostgreSQL
- Postman (for testing) or Swagger

### Step 1: Create the Project
```bash
dotnet new webapi -n SmartHomeAPI
cd SmartHomeAPI
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
```

## Database Design - Your Task

### Requirements
You need to design and create entities for a smart home system that tracks:

1. **Smart Devices** - Various IoT devices in the home +
2. **Device Categories** - Types/categories of devices  +
3. **Locations** - Where devices are placed in the home +
4. **Users** - People who interact with the system +
5. **Sensor Data** - Readings from devices over time +

### Entity Requirements

Think about what properties each entity should have and how they relate to each other. Consider:

**For Devices:**
- What information do you need to track about each device? +
- How do you know if a device is working or not?
- What about power consumption?

**For Device Categories:**
- How do you organize different types of devices?
- What characteristics do similar devices share?

**For Locations:**
- How do you organize spaces in a home?
- What properties might be useful for analytics?

**For Users:**
- What user information is needed?
- How do users interact with devices?

**For Sensor Data:**
- How do you store time-series data from sensors?
- What types of readings might devices produce?
- How do you make this data queryable?

### Relationships to Consider
- One-to-Many relationships (e.g., one room has many devices)
- Many-to-Many relationships (might users share devices?)
- Navigation properties for easy querying

### Design Challenge
Create your entities with these business rules in mind:
- Devices belong to specific locations
- Devices have categories/types +
- Sensor readings are timestamped and linked to devices
- Users can interact with devices
- System should track device health and usage patterns

### Step 2: Create DbContext
After designing your entities, create a DbContext class:

```csharp
public class SmartHomeContext : DbContext
{
    public SmartHomeContext(DbContextOptions<SmartHomeContext> options) : base(options) { }
    
    // Add your DbSet properties here for each entity
    // Example: public DbSet<YourEntity> YourEntities { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure your entity relationships here using Fluent API
        // Example: modelBuilder.Entity<EntityA>()
        //             .HasOne(a => a.EntityB)
        //             .WithMany(b => b.EntityAs)
        //             .HasForeignKey(a => a.EntityBId);
    }
}
```

### Step 3: Configure Connection String
In `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SmartHomeDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

In `Program.cs`:
```csharp
builder.Services.AddDbContext<SmartHomeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

## API Endpoints to Implement

### Phase 1: Basic CRUD Operations

#### Device Management
- `GET /api/devices` - Get all devices with related data
- `GET /api/devices/{id}` - Get specific device with details 
- `POST /api/devices` - Add new device 
- `PUT /api/devices/{id}` - Update device
- `DELETE /api/devices/{id}` - Delete device

#### Location Management
- `GET /api/locations` - Get all locations
- `GET /api/locations/{id}/devices` - Get devices in a location
- `POST /api/locations` - Add new location

#### User Management
- `GET /api/users` - Get all users +
- `POST /api/users` - Add new user +

### Phase 2: Data Operations

#### Sensor Readings
- `POST /api/devices/{id}/readings` - Add sensor reading
- `GET /api/devices/{id}/readings` - Get recent readings
- `GET /api/devices/{id}/readings/latest` - Get most recent reading

### Phase 3: Analytics & Reports

#### Basic Analytics
- `GET /api/analytics/energy-usage` - Total energy consumption
- `GET /api/analytics/device-status` - Online/offline device counts
- `GET /api/analytics/locations/usage` - Usage statistics by location
- `GET /api/analytics/devices/health` - Device health report

## Sample Data for Testing

Create a data seeding method in your DbContext. Here's a framework - fill in with your actual entities:

```csharp
public static void SeedData(SmartHomeContext context)
{
    // Check if data already exists
    if (context.YourMainEntity.Any())
        return;
    
    // Create sample data for your entities
    // Consider realistic smart home scenarios:
    // - Different types of devices (thermostats, lights, sensors)
    // - Multiple rooms/locations
    // - Different users with various roles
    // - Historical sensor data for testing analytics
    
    // Example pattern:
    // var categories = new List<YourCategoryEntity>
    // {
    //     new YourCategoryEntity { Name = "Climate Control", Description = "..." },
    //     new YourCategoryEntity { Name = "Lighting", Description = "..." },
    //     // ... more categories
    // };
    
    // context.YourCategories.AddRange(categories);
    // context.SaveChanges();
}
```

## Implementation Tips

### 1. Start Simple
Begin with basic CRUD operations before adding complex features.

### 2. Use DTOs (Data Transfer Objects)
Create separate classes for API requests/responses based on your entities:
```csharp
// Example - adapt to your entity design
public class CreateDeviceRequest
{
    public string Name { get; set; }
    public int CategoryId { get; set; }  // or whatever you call it
    public int LocationId { get; set; }   // or whatever you call it
    // Add other required properties
}

public class DeviceResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CategoryName { get; set; }  // from navigation property
    public string LocationName { get; set; }   // from navigation property
    public bool IsOnline { get; set; }
    // Add other properties you want to expose
}
```

### 3. Add Validation
Use Data Annotations:
```csharp
[Required]
[StringLength(100)]
public string Name { get; set; }

[Range(1, int.MaxValue)]
public int DeviceTypeId { get; set; }
```

### 4. Error Handling
Implement proper error responses:
```csharp
[HttpGet("{id}")]
public async Task<ActionResult<YourEntity>> GetEntity(int id)
{
    var entity = await _context.YourEntities.FindAsync(id);
    
    if (entity == null)
    {
        return NotFound($"Entity with ID {id} not found");
    }
    
    return entity;
}
```

## Testing Your API

### Sample Test Scenarios
Adapt these examples to your entity design:

1. **Create entities for each of your main types**
   ```json
   POST /api/locations
   {
     "name": "Living Room",
     "floor": 1,
     // other properties based on your design
   }
   ```

2. **Add devices with relationships**
   ```json
   POST /api/devices
   {
     "name": "Smart Thermostat",
     "categoryId": 1,  // or whatever your relationship is
     "locationId": 1,
     // other properties
   }
   ```

3. **Add sensor readings**
   ```json
   POST /api/devices/1/readings
   {
     "value": 72.5,
     "readingType": "Temperature",
     "unit": "°F"
     // adapt to your sensor data design
   }
   ```

4. **Test analytics endpoints**
   ```
   GET /api/analytics/energy-usage
   GET /api/analytics/device-status
   ```

## Bonus Challenges (Optional)

### Easy Bonuses
1. Add pagination to device listings
2. Implement search functionality (search devices by name)
3. Add filtering (e.g., only show online devices)

### Medium Bonuses
1. Create analytics endpoints that show trends over time
2. Implement user authentication with simple JWT tokens
3. Add device grouping functionality


## Deliverables
Submit the following:
1. Complete Visual Studio solution
2. Database migration files
3. README.md with setup instructions
5. Brief documentation of your API endpoints (swagger)


## Evaluation Criteria

Your project will be evaluated on:
1. **Entity Design** - Well-thought-out relationships and properties
2. **Code Quality** - Clean, readable code with proper naming
3. **API Design** - RESTful endpoints that make sense
4. **Functionality** - Working CRUD operations and basic analytics
5. **Testing** - Evidence of testing your endpoints
6. **Documentation** - Clear README and API documentation

Good luck with your project! Focus on getting the core functionality working first, then add advanced features.
