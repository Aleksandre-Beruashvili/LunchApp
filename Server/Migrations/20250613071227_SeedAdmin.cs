using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeCafeApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentCount",
                schema: "lunchapp",
                table: "Slots",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentCount",
                schema: "lunchapp",
                table: "Slots");
        }
    }
}
