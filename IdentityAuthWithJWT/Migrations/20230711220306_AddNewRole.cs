using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityAuthWithJWT.Migrations
{
    public partial class AddNewRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4aa0f81e-abb1-4b5e-8b9b-89c60e93b72f");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "5a4a3006-8f72-4d12-9b78-3408dffc9a22");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "72e6059b-ce4b-4a1b-95ee-12757bf7223f", "dfb0999c-dfad-40b9-a83c-91cf0d14cc01", "User", "USER" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9b3d3fbc-fe74-4528-b694-39bd7c699289", "629cbd8b-9f00-48d3-992e-78a0dc05fa9a", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cb9f8791-62b4-42c1-85d3-a575832e84f3", "51f5cff7-642a-45a2-8a4d-5c0b354e0b23", "Manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "72e6059b-ce4b-4a1b-95ee-12757bf7223f");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "9b3d3fbc-fe74-4528-b694-39bd7c699289");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "cb9f8791-62b4-42c1-85d3-a575832e84f3");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4aa0f81e-abb1-4b5e-8b9b-89c60e93b72f", "df6f3d50-abad-497e-a179-75b706267705", "User", "USER" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5a4a3006-8f72-4d12-9b78-3408dffc9a22", "efd918d4-2835-49a0-a54d-8f97ecb0b5ab", "Admin", "ADMIN" });
        }
    }
}
