using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCapsuleType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CapsuleTypeId",
                table: "Coffees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CapsuleTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapsuleTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coffees_CapsuleTypeId",
                table: "Coffees",
                column: "CapsuleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coffees_CapsuleTypes_CapsuleTypeId",
                table: "Coffees",
                column: "CapsuleTypeId",
                principalTable: "CapsuleTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coffees_CapsuleTypes_CapsuleTypeId",
                table: "Coffees");

            migrationBuilder.DropTable(
                name: "CapsuleTypes");

            migrationBuilder.DropIndex(
                name: "IX_Coffees_CapsuleTypeId",
                table: "Coffees");

            migrationBuilder.DropColumn(
                name: "CapsuleTypeId",
                table: "Coffees");
        }
    }
}
