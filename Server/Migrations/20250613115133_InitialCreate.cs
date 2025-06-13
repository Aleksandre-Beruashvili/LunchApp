using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeCafeApp.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScheduleDays",
                schema: "lunchapp",
                table: "Dishes",
                newName: "Portion");

            migrationBuilder.AddColumn<int>(
                name: "Leftover",
                schema: "lunchapp",
                table: "Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Leftover",
                schema: "lunchapp",
                table: "Dishes");

            migrationBuilder.RenameColumn(
                name: "Portion",
                schema: "lunchapp",
                table: "Dishes",
                newName: "ScheduleDays");
        }
    }
}
