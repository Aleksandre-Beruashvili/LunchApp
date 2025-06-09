using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LunchApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "lunchapp",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3240d562-454e-414b-808e-32666f4084fc", null, "Admin", "ADMIN" },
                    { "ac43c115-242e-4f34-aa1f-b14f1eeec846", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "lunchapp",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3240d562-454e-414b-808e-32666f4084fc");

            migrationBuilder.DeleteData(
                schema: "lunchapp",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac43c115-242e-4f34-aa1f-b14f1eeec846");
        }
    }
}
