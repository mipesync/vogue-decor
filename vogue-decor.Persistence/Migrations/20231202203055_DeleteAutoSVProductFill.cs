using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vogue_decor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeleteAutoSVProductFill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_SearchVector",
                table: "Products",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_SearchVector",
                table: "Products");
        }
    }
}
