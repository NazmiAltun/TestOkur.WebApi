namespace TestOkur.WebApi.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

    public partial class TemplatesDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "smses");

            migrationBuilder.DropTable(
                name: "templates");

            migrationBuilder.DropSequence(
                name: "sms_seq");

            migrationBuilder.DropSequence(
                name: "templates_seq");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "sms_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "templates_seq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "smses",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    body = table.Column<string>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    result = table.Column<string>(nullable: true),
                    subject = table.Column<string>(nullable: false),
                    successful = table.Column<bool>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                    receiver_value = table.Column<string>(maxLength: 20, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_smses", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "templates",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    comment = table.Column<string>(nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    path = table.Column<string>(nullable: false),
                    subject = table.Column<string>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                    name_value = table.Column<string>(maxLength: 100, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_templates", x => x.id);
                });
        }
    }
}
