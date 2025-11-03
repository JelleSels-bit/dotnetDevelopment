using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogRescue.Migrations
{
    /// <inheritdoc />
    public partial class updatemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Opmerking",
                table: "Hond",
                newName: "Opmerkingen");

            migrationBuilder.AddColumn<int>(
                name: "Geslacht",
                table: "Hond",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Geslacht",
                table: "Hond");

            migrationBuilder.RenameColumn(
                name: "Opmerkingen",
                table: "Hond",
                newName: "Opmerking");
        }
    }
}
