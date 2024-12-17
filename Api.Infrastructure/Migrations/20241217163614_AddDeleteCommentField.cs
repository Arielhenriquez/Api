using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteCommentField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeleteComment",
                table: "Vehicles",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteComment",
                table: "InventoryItems",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteComment",
                table: "Drivers",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteComment",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "DeleteComment",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "DeleteComment",
                table: "Drivers");
        }
    }
}
