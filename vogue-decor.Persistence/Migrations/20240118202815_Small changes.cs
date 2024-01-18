using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vogue_decor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Smallchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Preview",
                table: "Collections",
                newName: "Url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Collections",
                newName: "Preview");
        }
    }
}
