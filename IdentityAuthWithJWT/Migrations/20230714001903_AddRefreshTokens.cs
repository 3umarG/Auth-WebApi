using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityAuthWithJWT.Migrations
{
	public partial class AddRefreshTokens : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "RefreshTokens",
				columns: table => new
				{
					ApiUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
					ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RefreshToken", x => new { x.ApiUserId, x.Id });
					table.ForeignKey(
						name: "FK_RefreshToken_Users_ApiUserId",
						column: x => x.ApiUserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "RefreshToken");

		}
	}
}
