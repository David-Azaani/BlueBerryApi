using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueBerry_API.Migrations
{
    /// <inheritdoc />
    public partial class removeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardItems_ShoppingCards_ShoppingCardId",
                table: "CardItems");

            migrationBuilder.DropIndex(
                name: "IX_CardItems_ShoppingCardId",
                table: "CardItems");

            migrationBuilder.DropColumn(
                name: "ClientSecret",
                table: "ShoppingCards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientSecret",
                table: "ShoppingCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardItems_ShoppingCardId",
                table: "CardItems",
                column: "ShoppingCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardItems_ShoppingCards_ShoppingCardId",
                table: "CardItems",
                column: "ShoppingCardId",
                principalTable: "ShoppingCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
