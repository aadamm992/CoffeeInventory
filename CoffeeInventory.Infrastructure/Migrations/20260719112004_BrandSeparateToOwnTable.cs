using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BrandSeparateToOwnTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeCupSize_Coffees_CoffeesId",
                table: "CoffeeCupSize");

            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeCupSize_CupSizes_CupSizesId",
                table: "CoffeeCupSize");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoffeeCupSize",
                table: "CoffeeCupSize");

            migrationBuilder.DropSequence(
                name: "SerialSequence");

            migrationBuilder.RenameTable(
                name: "CoffeeCupSize",
                newName: "CoffeeCupSizes");

            migrationBuilder.RenameIndex(
                name: "IX_CoffeeCupSize_CupSizesId",
                table: "CoffeeCupSizes",
                newName: "IX_CoffeeCupSizes_CupSizesId");

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "Coffees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "Coffees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoffeeCupSizes",
                table: "CoffeeCupSizes",
                columns: new[] { "CoffeesId", "CupSizesId" });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coffees_BrandId",
                table: "Coffees",
                column: "BrandId");

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
                name: "FK_Coffees_Brands_BrandId",
                table: "Coffees",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeCupSizes_Coffees_CoffeesId",
                table: "CoffeeCupSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeCupSizes_CupSizes_CupSizesId",
                table: "CoffeeCupSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Coffees_Brands_BrandId",
                table: "Coffees");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Coffees_BrandId",
                table: "Coffees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoffeeCupSizes",
                table: "CoffeeCupSizes");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Coffees");

            migrationBuilder.RenameTable(
                name: "CoffeeCupSizes",
                newName: "CoffeeCupSize");

            migrationBuilder.RenameIndex(
                name: "IX_CoffeeCupSizes_CupSizesId",
                table: "CoffeeCupSize",
                newName: "IX_CoffeeCupSize_CupSizesId");

            migrationBuilder.CreateSequence<int>(
                name: "SerialSequence",
                startValue: 1000L);

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "Coffees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoffeeCupSize",
                table: "CoffeeCupSize",
                columns: new[] { "CoffeesId", "CupSizesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeCupSize_Coffees_CoffeesId",
                table: "CoffeeCupSize",
                column: "CoffeesId",
                principalTable: "Coffees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeCupSize_CupSizes_CupSizesId",
                table: "CoffeeCupSize",
                column: "CupSizesId",
                principalTable: "CupSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
