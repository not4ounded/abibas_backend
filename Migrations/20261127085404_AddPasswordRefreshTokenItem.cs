using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace abibas_back.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordRefreshTokenItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PasswordResetTokenItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TokenHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokenItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetTokenItems_UserItems_UserId",
                        column: x => x.UserId,
                        principalTable: "UserItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserItem_Email",
                table: "UserItems",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokenItems_UserId",
                table: "PasswordResetTokenItems",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordResetTokenItems");

            migrationBuilder.DropIndex(
                name: "IX_UserItem_Email",
                table: "UserItems");
        }
    }
}
