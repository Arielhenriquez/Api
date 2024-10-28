using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Collaborators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Supervisor = table.Column<string>(type: "longtext", nullable: false),
                    Department = table.Column<string>(type: "longtext", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborators", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    State = table.Column<string>(type: "longtext", nullable: true),
                    LicenseExpiration = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "longtext", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    Brand = table.Column<string>(type: "longtext", nullable: false),
                    Model = table.Column<string>(type: "longtext", nullable: false),
                    LicensePlate = table.Column<string>(type: "longtext", nullable: false),
                    InsuranceValidity = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    InsuranceType = table.Column<string>(type: "longtext", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InventoryRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    CollaboratorId = table.Column<Guid>(type: "char(36)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "longtext", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryRequests_Collaborators_CollaboratorId",
                        column: x => x.CollaboratorId,
                        principalTable: "Collaborators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TransportRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    CollaboratorId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Destination = table.Column<string>(type: "longtext", nullable: false),
                    RequestStatus = table.Column<string>(type: "longtext", nullable: false),
                    VehicleId = table.Column<Guid>(type: "char(36)", nullable: false),
                    DriverId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportRequests_Collaborators_CollaboratorId",
                        column: x => x.CollaboratorId,
                        principalTable: "Collaborators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransportRequests_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransportRequests_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InventoryRequestItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    InventoryRequestId = table.Column<Guid>(type: "char(36)", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryRequestItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryRequestItem_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryRequestItem_InventoryRequests_InventoryRequestId",
                        column: x => x.InventoryRequestId,
                        principalTable: "InventoryRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryRequestItem_InventoryItemId",
                table: "InventoryRequestItem",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryRequestItem_InventoryRequestId",
                table: "InventoryRequestItem",
                column: "InventoryRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryRequests_CollaboratorId",
                table: "InventoryRequests",
                column: "CollaboratorId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportRequests_CollaboratorId",
                table: "TransportRequests",
                column: "CollaboratorId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportRequests_DriverId",
                table: "TransportRequests",
                column: "DriverId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportRequests_VehicleId",
                table: "TransportRequests",
                column: "VehicleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryRequestItem");

            migrationBuilder.DropTable(
                name: "TransportRequests");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "InventoryRequests");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Collaborators");
        }
    }
}
