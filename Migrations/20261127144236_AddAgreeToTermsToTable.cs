using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace abibas_back.Migrations
{
    /// <inheritdoc />
    public partial class AddAgreeToTermsToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AgreeToTerms",
                table: "UserItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AgreeToTerms",
                table: "PendingRegistrationsItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgreeToTerms",
                table: "UserItems");

            migrationBuilder.DropColumn(
                name: "AgreeToTerms",
                table: "PendingRegistrationsItems");
        }
    }
}
