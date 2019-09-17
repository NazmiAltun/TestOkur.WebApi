namespace TestOkur.WebApi.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UserIdsRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_units_user_id",
                table: "units");

            migrationBuilder.DropIndex(
                name: "ix_students_user_id",
                table: "students");

            migrationBuilder.DropIndex(
                name: "ix_lessons_user_id",
                table: "lessons");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "units");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "students");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "score_formulas");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "lessons");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "exams");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "classrooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "units",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "students",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "score_formulas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "lessons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "exams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "classrooms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_units_user_id",
                table: "units",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_students_user_id",
                table: "students",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_lessons_user_id",
                table: "lessons",
                column: "user_id");
        }
    }
}
