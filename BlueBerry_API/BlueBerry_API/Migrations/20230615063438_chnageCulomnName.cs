using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueBerry_API.Migrations
{
    /// <inheritdoc />
    public partial class chnageCulomnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderType",
                table: "OrderHeaders",
                newName: "OrderDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "OrderHeaders",
                newName: "OrderType");
        }
    }
}
