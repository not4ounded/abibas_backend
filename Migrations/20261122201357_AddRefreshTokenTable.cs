using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace abibas_back.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "VerificationCodeItems");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "VerificationCodeItems",
                newName: "CodeHash");

            migrationBuilder.AddColumn<bool>(
                name: "IsInvalidated",
                table: "VerificationCodeItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmCode",
                table: "UserItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "UserItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RefreshTokenItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsRevoked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokenItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokenItems_UserItems_UserId",
                        column: x => x.UserId,
                        principalTable: "UserItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokenItems_UserId",
                table: "RefreshTokenItems",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokenItems");

            migrationBuilder.DropColumn(
                name: "IsInvalidated",
                table: "VerificationCodeItems");

            migrationBuilder.DropColumn(
                name: "ConfirmCode",
                table: "UserItems");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "UserItems");

            migrationBuilder.RenameColumn(
                name: "CodeHash",
                table: "VerificationCodeItems",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "VerificationCodeItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
