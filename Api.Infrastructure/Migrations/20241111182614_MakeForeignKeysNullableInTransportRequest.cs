using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeForeignKeysNullableInTransportRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransportRequests_Drivers_DriverId",
                table: "TransportRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportRequests_Vehicles_VehicleId",
                table: "TransportRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehicleId",
                table: "TransportRequests",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "TransportRequests",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportRequests_Drivers_DriverId",
                table: "TransportRequests",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportRequests_Vehicles_VehicleId",
                table: "TransportRequests",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransportRequests_Drivers_DriverId",
                table: "TransportRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportRequests_Vehicles_VehicleId",
                table: "TransportRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehicleId",
                table: "TransportRequests",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "TransportRequests",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TransportRequests_Drivers_DriverId",
                table: "TransportRequests",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransportRequests_Vehicles_VehicleId",
                table: "TransportRequests",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
