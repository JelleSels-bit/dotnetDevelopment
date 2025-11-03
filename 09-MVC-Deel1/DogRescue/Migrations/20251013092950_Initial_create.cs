using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogRescue.Migrations
{
    /// <inheritdoc />
    public partial class Initial_create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hond",
                columns: table => new
                {
                    HondId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeboorteDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opmerking = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hond", x => x.HondId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hond");
        }
    }
}
