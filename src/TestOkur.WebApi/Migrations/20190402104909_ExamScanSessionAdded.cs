namespace TestOkur.WebApi.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

    public partial class ExamScanSessionAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exam_scan_sessions",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    exam_id = table.Column<long>(nullable: true),
                    report_id = table.Column<Guid>(nullable: false),
                    by_camera = table.Column<bool>(nullable: false),
                    by_file = table.Column<bool>(nullable: false),
                    source = table.Column<string>(nullable: true),
                    start_date_time_utc = table.Column<DateTime>(nullable: false),
                    end_date_time_utc = table.Column<DateTime>(nullable: false),
                    scanned_student_count = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exam_scan_sessions", x => x.id);
                    table.ForeignKey(
                        name: "fk_exam_scan_sessions_exams_exam_id",
                        column: x => x.exam_id,
                        principalTable: "exams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_exam_scan_sessions_exam_id",
                table: "exam_scan_sessions",
                column: "exam_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exam_scan_sessions");
        }
    }
}
