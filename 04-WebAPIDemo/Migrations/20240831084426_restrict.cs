using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPIDemo.Migrations
{
    /// <inheritdoc />
    public partial class restrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestelling_Klant_KlantId",
                table: "Bestelling");

            migrationBuilder.DropForeignKey(
                name: "FK_Orderlijn_Bestelling_BestellingId",
                table: "Orderlijn");

            migrationBuilder.DropForeignKey(
                name: "FK_Orderlijn_Product_ProductId",
                table: "Orderlijn");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestelling_Klant_KlantId",
                table: "Bestelling",
                column: "KlantId",
                principalTable: "Klant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orderlijn_Bestelling_BestellingId",
                table: "Orderlijn",
                column: "BestellingId",
                principalTable: "Bestelling",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orderlijn_Product_ProductId",
                table: "Orderlijn",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestelling_Klant_KlantId",
                table: "Bestelling");

            migrationBuilder.DropForeignKey(
                name: "FK_Orderlijn_Bestelling_BestellingId",
                table: "Orderlijn");

            migrationBuilder.DropForeignKey(
                name: "FK_Orderlijn_Product_ProductId",
                table: "Orderlijn");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestelling_Klant_KlantId",
                table: "Bestelling",
                column: "KlantId",
                principalTable: "Klant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orderlijn_Bestelling_BestellingId",
                table: "Orderlijn",
                column: "BestellingId",
                principalTable: "Bestelling",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orderlijn_Product_ProductId",
                table: "Orderlijn",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
