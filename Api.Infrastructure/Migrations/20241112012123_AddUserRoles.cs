using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "TransportRequests");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "InventoryRequests");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "TransportRequests",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "TransportRequests",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PendingApprovalBy",
                table: "TransportRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "InventoryRequests",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "InventoryRequests",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PendingApprovalBy",
                table: "InventoryRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Roles",
                table: "Collaborators",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "TransportRequests");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "TransportRequests");

            migrationBuilder.DropColumn(
                name: "PendingApprovalBy",
                table: "TransportRequests");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "InventoryRequests");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "InventoryRequests");

            migrationBuilder.DropColumn(
                name: "PendingApprovalBy",
                table: "InventoryRequests");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Collaborators");

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "TransportRequests",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "InventoryRequests",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
