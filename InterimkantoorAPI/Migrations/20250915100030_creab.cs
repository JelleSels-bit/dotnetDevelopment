using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterimkantoorAPI.Migrations
{
    /// <inheritdoc />
    public partial class creab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    locatie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EindDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsWerkschoenen = table.Column<bool>(type: "bit", nullable: false),
                    IsBadge = table.Column<bool>(type: "bit", nullable: false),
                    IsKleding = table.Column<bool>(type: "bit", nullable: false),
                    AantalPlaatsen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Klant",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Voornaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gemeente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postcode = table.Column<int>(type: "int", nullable: false),
                    Straat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Huisnummer = table.Column<int>(type: "int", nullable: false),
                    Bankrekeningnummer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klant", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "Klant");
        }
    }
}
