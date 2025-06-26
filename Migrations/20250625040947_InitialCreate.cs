using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Home_IoT_Device_Management_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Manufacturer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FirmwareVersion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PowerSource = table.Column<string>(type: "text", nullable: false),
                    RequiresInternet = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CommunicationProtocol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DeviceType = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    FloorNumber = table.Column<int>(type: "integer", nullable: true),
                    RoomId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HashedPassword = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    InstalledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SerialNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PowerConsumption = table.Column<float>(type: "real", precision: 10, scale: 2, nullable: true),
                    MACAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastCommunicationAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsageCount = table.Column<int>(type: "integer", nullable: true),
                    LastUsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeviceSetting_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceSetting_Brightness = table.Column<int>(type: "integer", nullable: true),
                    DeviceSetting_Volume = table.Column<int>(type: "integer", nullable: true),
                    DeviceSetting_TemperatureThreshold = table.Column<int>(type: "integer", nullable: true),
                    DeviceSetting_AutoShutdown = table.Column<bool>(type: "boolean", nullable: true),
                    DeviceSetting_MotionSensitivity = table.Column<int>(type: "integer", nullable: true),
                    DeviceSetting_UpdateIntervalSeconds = table.Column<int>(type: "integer", nullable: true),
                    DeviceCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_DeviceCategories_DeviceCategoryId",
                        column: x => x.DeviceCategoryId,
                        principalTable: "DeviceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Devices_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Voltage = table.Column<float>(type: "real", precision: 10, scale: 2, nullable: true),
                    Current = table.Column<float>(type: "real", precision: 10, scale: 2, nullable: true),
                    PowerConsumptionWatts = table.Column<float>(type: "real", precision: 10, scale: 2, nullable: true),
                    BatteryLevel = table.Column<int>(type: "integer", nullable: true),
                    SignalStrengthDb = table.Column<float>(type: "real", precision: 6, scale: 2, nullable: true),
                    Temperature = table.Column<float>(type: "real", precision: 5, scale: 2, nullable: true),
                    Humidity = table.Column<float>(type: "real", precision: 5, scale: 2, nullable: true),
                    Pressure = table.Column<float>(type: "real", precision: 6, scale: 2, nullable: true),
                    LightLevel = table.Column<float>(type: "real", precision: 6, scale: 2, nullable: true),
                    CO2Level = table.Column<float>(type: "real", precision: 6, scale: 2, nullable: true),
                    MotionDetected = table.Column<bool>(type: "boolean", nullable: true),
                    SoundLevel = table.Column<float>(type: "real", precision: 6, scale: 2, nullable: true),
                    AirQualityIndex = table.Column<int>(type: "integer", nullable: true),
                    UptimeSeconds = table.Column<long>(type: "bigint", nullable: true),
                    RecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorDatas_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDevicePermissions",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Permission = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDevicePermissions", x => new { x.UserId, x.DeviceId });
                    table.ForeignKey(
                        name: "FK_UserDevicePermissions_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDevicePermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceCategoryId",
                table: "Devices",
                column: "DeviceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_LocationId",
                table: "Devices",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorDatas_DeviceId",
                table: "SensorDatas",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevicePermissions_DeviceId",
                table: "UserDevicePermissions",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorDatas");

            migrationBuilder.DropTable(
                name: "UserDevicePermissions");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "DeviceCategories");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
