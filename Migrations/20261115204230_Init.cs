using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace abibas_back.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckoutItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    Region = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Street = table.Column<string>(type: "TEXT", nullable: false),
                    StreetNumber = table.Column<string>(type: "TEXT", nullable: false),
                    ApartmentNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Delivery = table.Column<int>(type: "INTEGER", nullable: false),
                    CreditCardNumber = table.Column<string>(type: "TEXT", nullable: false),
                    CardExpired = table.Column<string>(type: "TEXT", nullable: false),
                    CVV = table.Column<string>(type: "TEXT", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    PromoCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckoutItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommentItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    AuthorName = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    Last4 = table.Column<string>(type: "TEXT", nullable: false),
                    Brand = table.Column<string>(type: "TEXT", nullable: false),
                    PaymentToken = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ShortDescription = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Sizes = table.Column<string>(type: "TEXT", nullable: false),
                    Colors = table.Column<string>(type: "TEXT", nullable: false),
                    IsFreeShipping = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsFreeShippingAndReturns = table.Column<bool>(type: "INTEGER", nullable: false),
                    ProductTags = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNum = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VerificationCodeItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationCodeItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    URL = table.Column<string>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Caption = table.Column<string>(type: "TEXT", nullable: false),
                    ProductItemId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_ProductItems_ProductItemId",
                        column: x => x.ProductItemId,
                        principalTable: "ProductItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalDetailItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalDetailItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalDetailItems_ProductItems_ProductItemId",
                        column: x => x.ProductItemId,
                        principalTable: "ProductItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    SelectedSize = table.Column<int>(type: "INTEGER", nullable: false),
                    SelectedColor = table.Column<int>(type: "INTEGER", nullable: false),
                    UserItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    CheckoutItemId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_CheckoutItems_CheckoutItemId",
                        column: x => x.CheckoutItemId,
                        principalTable: "CheckoutItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartItems_ProductItems_ProductItemId",
                        column: x => x.ProductItemId,
                        principalTable: "ProductItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_UserItems_UserItemId",
                        column: x => x.UserItemId,
                        principalTable: "UserItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CheckoutItemId",
                table: "CartItems",
                column: "CheckoutItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductItemId",
                table: "CartItems",
                column: "ProductItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserItemId",
                table: "CartItems",
                column: "UserItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductItemId",
                table: "ProductImages",
                column: "ProductItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalDetailItems_ProductItemId",
                table: "TechnicalDetailItems",
                column: "ProductItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "CommentItems");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "TechnicalDetailItems");

            migrationBuilder.DropTable(
                name: "VerificationCodeItems");

            migrationBuilder.DropTable(
                name: "CheckoutItems");

            migrationBuilder.DropTable(
                name: "UserItems");

            migrationBuilder.DropTable(
                name: "ProductItems");
        }
    }
}
