using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace vogue_decor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Filldataandcategoryentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChandelierTypes");

            migrationBuilder.DropColumn(
                name: "ChandelierTypes",
                table: "Products");

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Styles",
                type: "tsvector",
                nullable: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector")
                .Annotation("Npgsql:TsVectorConfig", "russian")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Name" })
                .OldAnnotation("Npgsql:TsVectorConfig", "russian")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "Name" });

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "ProductTypes",
                type: "tsvector",
                nullable: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector")
                .Annotation("Npgsql:TsVectorConfig", "russian")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Name" })
                .OldAnnotation("Npgsql:TsVectorConfig", "russian")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "Name" });

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Materials",
                type: "tsvector",
                nullable: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector")
                .Annotation("Npgsql:TsVectorConfig", "russian")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Name" })
                .OldAnnotation("Npgsql:TsVectorConfig", "russian")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "Name" });

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Colors",
                type: "tsvector",
                nullable: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector")
                .Annotation("Npgsql:TsVectorConfig", "russian")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Name", "EngName" })
                .OldAnnotation("Npgsql:TsVectorConfig", "russian")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "Name", "EngName" });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true)
                        .Annotation("Npgsql:TsVectorConfig", "russian")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "Name" }),
                    ProductTypeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ProductTypeId" },
                values: new object[,]
                {
                    { 1, "Люстры", 1 },
                    { 2, "Бра", 1 },
                    { 3, "Настольные лампы", 1 },
                    { 4, "Торшеры", 1 },
                    { 5, "Подвесные светильники", 1 },
                    { 6, "Потолочные светильники", 1 },
                    { 7, "Уличный свет", 1 },
                    { 8, "Подсветка для картин", 1 },
                    { 9, "Треки и споты", 1 },
                    { 10, "Аксессуары к светильникам", 1 },
                    { 11, "Диваны", 2 },
                    { 12, "Кресла", 2 },
                    { 13, "Столы", 2 },
                    { 14, "Стулья", 2 },
                    { 15, "Комоды", 2 },
                    { 16, "Консоли", 2 },
                    { 17, "Кровати", 2 },
                    { 18, "Матрасы", 2 },
                    { 19, "Пуфы и банкетки", 2 },
                    { 20, "Панно / арт объект", 3 },
                    { 21, "С принтами", 3 },
                    { 22, "Солнышко", 3 },
                    { 23, "С деревом", 3 },
                    { 24, "Дизайнерские с металлом", 3 },
                    { 25, "Классические", 3 },
                    { 26, "Настольные", 3 },
                    { 27, "Напольные", 3 },
                    { 28, "Прямоугольные", 3 },
                    { 29, "Круглые", 3 },
                    { 30, "Прямоугольные", 4 },
                    { 31, "Квадратные", 4 },
                    { 32, "Круглые", 4 },
                    { 33, "Овальные", 4 },
                    { 34, "Дорожки", 4 },
                    { 35, "Нестандартные", 4 },
                    { 36, "Дизайнерские тарелки", 5 },
                    { 37, "Стремянки и скамьи", 5 },
                    { 38, "Сушилки", 5 },
                    { 39, "Гладильные доски", 5 },
                    { 40, "Вешалки напольные", 5 },
                    { 41, "Вешалки настенные", 5 },
                    { 42, "Аксессуары для ванной", 5 },
                    { 43, "Ложки для обуви", 5 },
                    { 44, "Вазы и подсвечники", 5 },
                    { 45, "Декоративные подушки", 5 },
                    { 46, "Пледы", 5 },
                    { 47, "Покрывала", 5 },
                    { 48, "Современные игрушки и статуэтки", 6 },
                    { 49, "Часы", 6 },
                    { 50, "Настольные и настенные игры", 6 },
                    { 51, "Зонты", 6 },
                    { 52, "Подставки для зонтов", 6 },
                    { 53, "Ложки для обуви", 6 },
                    { 54, "Держатели книг", 6 },
                    { 55, "Арт-объекты", 7 },
                    { 56, "Картины авторские", 7 },
                    { 57, "Постеры", 7 },
                    { 58, "Панно с лего", 7 },
                    { 59, "Панно в спортивном стиле", 7 },
                    { 60, "Репродукция", 7 },
                    { 61, "Абажуры", 8 },
                    { 62, "Колпаки и крепления", 8 },
                    { 63, "Лампочки", 8 },
                    { 64, "Светодиодные ленты и подсветки", 8 }
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Id", "EngName", "Name" },
                values: new object[,]
                {
                    { 1, "Gold", "Золотой" },
                    { 2, "Bronze", "Бронзовый" },
                    { 3, "Silver", "Серебристый" },
                    { 4, "Nickel", "Никель" },
                    { 5, "Chrome", "Хром" },
                    { 6, "White", "Белый" },
                    { 7, "Black", "Чёрный" },
                    { 8, "Clear", "Прозрачный" },
                    { 9, "Beige", "Бежевый" },
                    { 10, "Light blue", "Голубой" },
                    { 11, "Yellow", "Жёлтый" },
                    { 12, "Green", "Зелёный" },
                    { 13, "Brown", "Коричневый" },
                    { 14, "Red", "Красный" },
                    { 15, "Orange", "Оранжевый" },
                    { 16, "Pink", "Розовый" },
                    { 17, "Gray", "Серый" },
                    { 18, "Blue", "Синий" },
                    { 19, "Purple", "Фиолетовый" }
                });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Металл" },
                    { 2, "Стекло" },
                    { 3, "Дерево" },
                    { 4, "Смешанное" },
                    { 5, "Шёлк" },
                    { 6, "Арт-шёлк" },
                    { 7, "Шерсть" },
                    { 8, "Вискоза" },
                    { 9, "Искусственная нить" },
                    { 10, "Хлоповый" },
                    { 11, "Искусственный" },
                    { 12, "Стеклянный" },
                    { 13, "Бумажный" },
                    { 14, "Пластиковый" },
                    { 15, "Гипс" },
                    { 16, "Полиэстер" },
                    { 17, "Латунь" },
                    { 18, "Бронза" }
                });

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Свет" },
                    { 2, "Мебель" },
                    { 3, "Зеркала" },
                    { 4, "Ковры" },
                    { 5, "Товары для дома" },
                    { 6, "Аксессуары" },
                    { 7, "Картины и пано" },
                    { 8, "Аксессуары к светильникам" }
                });

            migrationBuilder.InsertData(
                table: "Styles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Классика" },
                    { 2, "Нео-классика" },
                    { 3, "Лофт" },
                    { 4, "Поп-Арт" },
                    { 5, "Модерн" },
                    { 6, "Арт-деко" },
                    { 7, "Минималист" },
                    { 8, "ХайТек" },
                    { 9, "Скандинавский" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Id",
                table: "Categories",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SearchVector",
                table: "Categories",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Styles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Styles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Styles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Styles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Styles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Styles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Styles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Styles",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Styles",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Styles",
                type: "tsvector",
                nullable: false,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldNullable: true)
                .Annotation("Npgsql:TsVectorConfig", "russian")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Name" })
                .OldAnnotation("Npgsql:TsVectorConfig", "russian")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "Name" });

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "ProductTypes",
                type: "tsvector",
                nullable: false,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldNullable: true)
                .Annotation("Npgsql:TsVectorConfig", "russian")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Name" })
                .OldAnnotation("Npgsql:TsVectorConfig", "russian")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "Name" });

            migrationBuilder.AddColumn<int[]>(
                name: "ChandelierTypes",
                table: "Products",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Materials",
                type: "tsvector",
                nullable: false,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldNullable: true)
                .Annotation("Npgsql:TsVectorConfig", "russian")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Name" })
                .OldAnnotation("Npgsql:TsVectorConfig", "russian")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "Name" });

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Colors",
                type: "tsvector",
                nullable: false,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldNullable: true)
                .Annotation("Npgsql:TsVectorConfig", "russian")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Name", "EngName" })
                .OldAnnotation("Npgsql:TsVectorConfig", "russian")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "Name", "EngName" });

            migrationBuilder.CreateTable(
                name: "ChandelierTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false)
                        .Annotation("Npgsql:TsVectorConfig", "russian")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "Name" })
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChandelierTypes", x => x.Id);
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
    }
}
