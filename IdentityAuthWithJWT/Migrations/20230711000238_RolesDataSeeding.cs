using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityAuthWithJWT.Migrations
{
    public partial class RolesDataSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4aa0f81e-abb1-4b5e-8b9b-89c60e93b72f", "df6f3d50-abad-497e-a179-75b706267705", "User", "USER" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5a4a3006-8f72-4d12-9b78-3408dffc9a22", "efd918d4-2835-49a0-a54d-8f97ecb0b5ab", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4aa0f81e-abb1-4b5e-8b9b-89c60e93b72f");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "5a4a3006-8f72-4d12-9b78-3408dffc9a22");
        }
    }
}
