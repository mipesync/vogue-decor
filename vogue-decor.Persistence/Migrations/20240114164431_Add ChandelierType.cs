using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace vogue_decor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddChandelierType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Discount",
                table: "Products",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int[]>(
                name: "ChandelierTypes",
                table: "Products",
                type: "integer[]",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChandelierTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true)
                        .Annotation("Npgsql:TsVectorConfig", "russian")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "Name" })
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChandelierTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ChandelierTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Большие" },
                    { 2, "С хрусталями" },
                    { 3, "С абажурами" },
                    { 4, "Подвесные" },
                    { 5, "С потолочным креплением" },
                    { 6, "Овальные" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChandelierTypes_Id",
                table: "ChandelierTypes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChandelierTypes_SearchVector",
                table: "ChandelierTypes",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChandelierTypes");

            migrationBuilder.DropColumn(
                name: "ChandelierTypes",
                table: "Products");

            migrationBuilder.AlterColumn<long>(
                name: "Discount",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
