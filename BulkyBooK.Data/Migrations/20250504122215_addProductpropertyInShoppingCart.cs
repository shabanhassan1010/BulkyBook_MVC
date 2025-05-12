using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkyBook.Data.Migrations
{
    /// <inheritdoc />
    public partial class addProductpropertyInShoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ShopingCarts_ProductId",
                table: "ShopingCarts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopingCarts_Products_ProductId",
                table: "ShopingCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopingCarts_Products_ProductId",
                table: "ShopingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShopingCarts_ProductId",
                table: "ShopingCarts");
        }
    }
}
