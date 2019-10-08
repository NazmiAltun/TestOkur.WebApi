namespace TestOkur.WebApi.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class CreatedBy_Indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_users_created_by",
                table: "users",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_units_created_by",
                table: "units",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_subjects_created_by",
                table: "subjects",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_students_created_by",
                table: "students",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_score_formulas_created_by",
                table: "score_formulas",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_lessons_created_by",
                table: "lessons",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_exams_created_by",
                table: "exams",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_exam_types_created_by",
                table: "exam_types",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_exam_scan_sessions_created_by",
                table: "exam_scan_sessions",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_contacts_created_by",
                table: "contacts",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_classrooms_created_by",
                table: "classrooms",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_appsettings_created_by",
                table: "appsettings",
                column: "created_by");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_created_by",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_units_created_by",
                table: "units");

            migrationBuilder.DropIndex(
                name: "ix_subjects_created_by",
                table: "subjects");

            migrationBuilder.DropIndex(
                name: "ix_students_created_by",
                table: "students");

            migrationBuilder.DropIndex(
                name: "ix_score_formulas_created_by",
                table: "score_formulas");

            migrationBuilder.DropIndex(
                name: "ix_lessons_created_by",
                table: "lessons");

            migrationBuilder.DropIndex(
                name: "ix_exams_created_by",
                table: "exams");

            migrationBuilder.DropIndex(
                name: "ix_exam_types_created_by",
                table: "exam_types");

            migrationBuilder.DropIndex(
                name: "ix_exam_scan_sessions_created_by",
                table: "exam_scan_sessions");

            migrationBuilder.DropIndex(
                name: "ix_contacts_created_by",
                table: "contacts");

            migrationBuilder.DropIndex(
                name: "ix_classrooms_created_by",
                table: "classrooms");

            migrationBuilder.DropIndex(
                name: "ix_appsettings_created_by",
                table: "appsettings");
        }
    }
}
