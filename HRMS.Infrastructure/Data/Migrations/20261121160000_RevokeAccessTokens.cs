using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Infrastructure.Data.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20261121160000_RevokeAccessTokens")]
public partial class RevokeAccessTokens : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "RevokedAccessTokens",
            columns: table => new
            {
                TokenId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_RevokedAccessTokens", token => token.TokenId));

        migrationBuilder.CreateIndex(
            name: "IX_RevokedAccessTokens_ExpiresAt",
            table: "RevokedAccessTokens",
            column: "ExpiresAt");
    }

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable("RevokedAccessTokens");
}
