namespace TestOkur.WebApi.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

    public partial class SettingsRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appsettings");

            migrationBuilder.DropSequence(
                name: "appsettings_seq");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "appsettings_seq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "appsettings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    comment = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<int>(type: "integer", nullable: false),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    name_value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_appsettings", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_appsettings_created_by",
                table: "appsettings",
                column: "created_by");
        }
    }
}
