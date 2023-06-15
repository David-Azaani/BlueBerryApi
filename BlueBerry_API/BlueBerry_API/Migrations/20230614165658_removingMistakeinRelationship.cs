using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueBerry_API.Migrations
{
    /// <inheritdoc />
    public partial class removingMistakeinRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripPaymentIntentId",
                table: "ShoppingCards");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardItems_ShoppingCards_ShoppingCardId",
                table: "CardItems");

            migrationBuilder.DropIndex(
                name: "IX_CardItems_ShoppingCardId",
                table: "CardItems");

            migrationBuilder.AddColumn<string>(
                name: "StripPaymentIntentId",
                table: "ShoppingCards",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
