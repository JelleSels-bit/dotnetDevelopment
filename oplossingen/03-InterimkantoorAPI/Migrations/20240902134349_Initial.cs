using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterimkantoorAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
                    Omschrijving = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StartDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EindDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Locatie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                    Naam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Voornaam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gemeente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Straat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Huisnummer = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Bankrekeningnummer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KlantJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KlantJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KlantJobs_Job_JobId",
                        column: x => x.JobId,
                        principalTable: "Job",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KlantJobs_Klant_KlantId",
                        column: x => x.KlantId,
                        principalTable: "Klant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KlantJobs_JobId",
                table: "KlantJobs",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_KlantJobs_KlantId",
                table: "KlantJobs",
                column: "KlantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KlantJobs");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "Klant");
        }
    }
}
