using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCupSizeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CupSizes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VolumeMl = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CupSizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeCupSize",
                columns: table => new
                {
                    CoffeesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CupSizesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeCupSize", x => new { x.CoffeesId, x.CupSizesId });
                    table.ForeignKey(
                        name: "FK_CoffeeCupSize_Coffees_CoffeesId",
                        column: x => x.CoffeesId,
                        principalTable: "Coffees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoffeeCupSize_CupSizes_CupSizesId",
                        column: x => x.CupSizesId,
                        principalTable: "CupSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeCupSize_CupSizesId",
                table: "CoffeeCupSize",
                column: "CupSizesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeCupSize");

            migrationBuilder.DropTable(
                name: "CupSizes");
        }
    }
}
