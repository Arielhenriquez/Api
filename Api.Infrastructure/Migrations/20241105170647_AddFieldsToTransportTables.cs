using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToTransportTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Drivers");

            migrationBuilder.AddColumn<DateTime>(
                name: "DepartureDateTime",
                table: "TransportRequests",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeparturePoint",
                table: "TransportRequests",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPeople",
                table: "TransportRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "TransportRequests",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Drivers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartureDateTime",
                table: "TransportRequests");

            migrationBuilder.DropColumn(
                name: "DeparturePoint",
                table: "TransportRequests");

            migrationBuilder.DropColumn(
                name: "NumberOfPeople",
                table: "TransportRequests");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "TransportRequests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Drivers");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Drivers",
                type: "longtext",
                nullable: true);
        }
    }
}
