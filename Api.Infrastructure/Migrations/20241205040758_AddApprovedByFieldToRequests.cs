using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApprovedByFieldToRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "TransportRequests");

            migrationBuilder.AddColumn<DateTime?>(
                name: "StatusChangedDate",
                table: "TransportRequests",
                type: "datetime",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "InventoryRequests");

            migrationBuilder.AddColumn<DateTime?>(
                name: "StatusChangedDate",
                table: "InventoryRequests",
                type: "datetime",
                nullable: true);


            migrationBuilder.AddColumn<string>(
                name: "ApprovedOrRejectedBy",
                table: "TransportRequests",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedOrRejectedBy",
                table: "InventoryRequests",
                type: "json",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedOrRejectedBy",
                table: "TransportRequests");

            migrationBuilder.DropColumn(
                name: "ApprovedOrRejectedBy",
                table: "InventoryRequests");

            migrationBuilder.RenameColumn(
                name: "StatusChangedDate",
                table: "TransportRequests",
                newName: "ApprovedDate");

            migrationBuilder.RenameColumn(
                name: "StatusChangedDate",
                table: "InventoryRequests",
                newName: "ApprovedDate");
        }
    }
}
