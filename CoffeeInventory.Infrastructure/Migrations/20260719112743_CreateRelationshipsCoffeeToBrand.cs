using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateRelationshipsCoffeeToBrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coffees_Brands_BrandId",
                table: "Coffees");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Coffees");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Brands",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Coffees_Brands_BrandId",
                table: "Coffees",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coffees_Brands_BrandId",
                table: "Coffees");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Coffees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_Coffees_Brands_BrandId",
                table: "Coffees",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");
        }
    }
}
