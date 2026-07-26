using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeCupSizes_Coffees_CoffeesId",
                table: "CoffeeCupSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeCupSizes_CupSizes_CupSizesId",
                table: "CoffeeCupSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Coffees_CapsuleTypes_CapsuleTypeId",
                table: "Coffees");

            migrationBuilder.RenameColumn(
                name: "CupSizesId",
                table: "CoffeeCupSizes",
                newName: "CupSizeId");

            migrationBuilder.RenameColumn(
                name: "CoffeesId",
                table: "CoffeeCupSizes",
                newName: "CoffeeId");

            migrationBuilder.RenameIndex(
                name: "IX_CoffeeCupSizes_CupSizesId",
                table: "CoffeeCupSizes",
                newName: "IX_CoffeeCupSizes_CupSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeCupSizes_Coffees_CoffeeId",
                table: "CoffeeCupSizes",
                column: "CoffeeId",
                principalTable: "Coffees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeCupSizes_CupSizes_CupSizeId",
                table: "CoffeeCupSizes",
                column: "CupSizeId",
                principalTable: "CupSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Coffees_CapsuleTypes_CapsuleTypeId",
                table: "Coffees",
                column: "CapsuleTypeId",
                principalTable: "CapsuleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeCupSizes_Coffees_CoffeeId",
                table: "CoffeeCupSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeCupSizes_CupSizes_CupSizeId",
                table: "CoffeeCupSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Coffees_CapsuleTypes_CapsuleTypeId",
                table: "Coffees");

            migrationBuilder.RenameColumn(
                name: "CupSizeId",
                table: "CoffeeCupSizes",
                newName: "CupSizesId");

            migrationBuilder.RenameColumn(
                name: "CoffeeId",
                table: "CoffeeCupSizes",
                newName: "CoffeesId");

            migrationBuilder.RenameIndex(
                name: "IX_CoffeeCupSizes_CupSizeId",
                table: "CoffeeCupSizes",
                newName: "IX_CoffeeCupSizes_CupSizesId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeCupSizes_Coffees_CoffeesId",
                table: "CoffeeCupSizes",
                column: "CoffeesId",
                principalTable: "Coffees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeCupSizes_CupSizes_CupSizesId",
                table: "CoffeeCupSizes",
                column: "CupSizesId",
                principalTable: "CupSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Coffees_CapsuleTypes_CapsuleTypeId",
                table: "Coffees",
                column: "CapsuleTypeId",
                principalTable: "CapsuleTypes",
                principalColumn: "Id");
        }
    }
}
