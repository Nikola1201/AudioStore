using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_OrderDetails_OrderDetailsOrderID",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_OrderDetailsOrderID",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "OrderDetailsOrderID",
                table: "ShoppingCarts");

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailsID",
                table: "ShoppingCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_OrderDetailsID",
                table: "ShoppingCarts",
                column: "OrderDetailsID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_OrderDetails_OrderDetailsID",
                table: "ShoppingCarts",
                column: "OrderDetailsID",
                principalTable: "OrderDetails",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_OrderDetails_OrderDetailsID",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_OrderDetailsID",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "OrderDetailsID",
                table: "ShoppingCarts");

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailsOrderID",
                table: "ShoppingCarts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_OrderDetailsOrderID",
                table: "ShoppingCarts",
                column: "OrderDetailsOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_OrderDetails_OrderDetailsOrderID",
                table: "ShoppingCarts",
                column: "OrderDetailsOrderID",
                principalTable: "OrderDetails",
                principalColumn: "OrderID");
        }
    }
}
