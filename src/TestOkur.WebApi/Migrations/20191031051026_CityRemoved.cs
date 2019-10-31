namespace TestOkur.WebApi.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class CityRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_cities_city_id",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "fk_users_districts_district_id",
                table: "users");

            migrationBuilder.DropTable(
                name: "districts");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropIndex(
                name: "ix_users_city_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_users_district_id",
                table: "users");

            migrationBuilder.AlterColumn<int>(
                name: "district_id",
                table: "users",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "city_id",
                table: "users",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "district_id",
                table: "users",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "city_id",
                table: "users",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name_value = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "districts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    city_id = table.Column<long>(type: "bigint", nullable: true),
                    name_value = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_districts", x => x.id);
                    table.ForeignKey(
                        name: "fk_districts_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_users_city_id",
                table: "users",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_district_id",
                table: "users",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "ix_districts_city_id",
                table: "districts",
                column: "city_id");

            migrationBuilder.AddForeignKey(
                name: "fk_users_cities_city_id",
                table: "users",
                column: "city_id",
                principalTable: "cities",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_users_districts_district_id",
                table: "users",
                column: "district_id",
                principalTable: "districts",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
