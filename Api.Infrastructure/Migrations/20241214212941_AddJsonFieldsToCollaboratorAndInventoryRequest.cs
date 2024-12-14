using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJsonFieldsToCollaboratorAndInventoryRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
              name: "ApprovalHistory",
              table: "InventoryRequests",
              type: "json",
              nullable: true);
            
            migrationBuilder.DropColumn(
                name: "ApprovedOrRejectedBy",
                table: "InventoryRequests");

            migrationBuilder.AddColumn<string>(
                name: "RequestCode",
                table: "TransportRequests",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "RequestCode",
                table: "InventoryRequests",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "AcquisitionObjectAccount",
                table: "InventoryItems",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "InventoryItems",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "InstitutionalCode",
                table: "InventoryItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequestedQuantity",
                table: "InventoryItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Section",
                table: "InventoryItems",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseObjectAccount",
                table: "InventoryItems",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Approvers",
                table: "Collaborators",
                type: "json",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestCode",
                table: "TransportRequests");

            migrationBuilder.DropColumn(
                name: "RequestCode",
                table: "InventoryRequests");

            migrationBuilder.DropColumn(
                name: "AcquisitionObjectAccount",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "InstitutionalCode",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "RequestedQuantity",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "Section",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "WarehouseObjectAccount",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "Approvers",
                table: "Collaborators");

            migrationBuilder.RenameColumn(
                name: "ApprovalHistory",
                table: "InventoryRequests",
                newName: "ApprovedOrRejectedBy");
        }
    }
}
