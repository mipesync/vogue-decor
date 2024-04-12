using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vogue_decor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationBrandCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "Collections",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Collections_BrandId",
                table: "Collections",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collections_Brands_BrandId",
                table: "Collections",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql("ALTER TABLE \"Products\" ALTER COLUMN \"Width\" TYPE numeric[] USING ARRAY[\"Width\"];");
            migrationBuilder.Sql("ALTER TABLE \"Products\" ALTER COLUMN \"Height\" TYPE numeric[] USING ARRAY[\"Height\"];");
            migrationBuilder.Sql("ALTER TABLE \"Products\" ALTER COLUMN \"Length\" TYPE numeric[] USING ARRAY[\"Length\"];");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collections_Brands_BrandId",
                table: "Collections");

            migrationBuilder.DropIndex(
                name: "IX_Collections_BrandId",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Collections");
        }
    }
}
