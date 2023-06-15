using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueBerry_API.Migrations
{
    /// <inheritdoc />
    public partial class TableNamesedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetailsEnumerable_MenuItems_MenuItemId",
                table: "OrderDetailsEnumerable");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetailsEnumerable_OrderHeaders_OrderHeaderId",
                table: "OrderDetailsEnumerable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetailsEnumerable",
                table: "OrderDetailsEnumerable");

            migrationBuilder.RenameTable(
                name: "OrderDetailsEnumerable",
                newName: "OrderDetails");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetailsEnumerable_OrderHeaderId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_OrderHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetailsEnumerable_MenuItemId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_MenuItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                column: "OrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_MenuItems_MenuItemId",
                table: "OrderDetails",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                table: "OrderDetails",
                column: "OrderHeaderId",
                principalTable: "OrderHeaders",
                principalColumn: "OrderHeaderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_MenuItems_MenuItemId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                table: "OrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.RenameTable(
                name: "OrderDetails",
                newName: "OrderDetailsEnumerable");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_OrderHeaderId",
                table: "OrderDetailsEnumerable",
                newName: "IX_OrderDetailsEnumerable_OrderHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_MenuItemId",
                table: "OrderDetailsEnumerable",
                newName: "IX_OrderDetailsEnumerable_MenuItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetailsEnumerable",
                table: "OrderDetailsEnumerable",
                column: "OrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetailsEnumerable_MenuItems_MenuItemId",
                table: "OrderDetailsEnumerable",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetailsEnumerable_OrderHeaders_OrderHeaderId",
                table: "OrderDetailsEnumerable",
                column: "OrderHeaderId",
                principalTable: "OrderHeaders",
                principalColumn: "OrderHeaderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
