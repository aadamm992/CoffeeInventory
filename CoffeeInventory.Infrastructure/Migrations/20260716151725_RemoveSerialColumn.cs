using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSerialColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Serial",
                table: "Coffees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Serial",
                table: "Coffees",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR SerialSequence");
        }
    }
}
