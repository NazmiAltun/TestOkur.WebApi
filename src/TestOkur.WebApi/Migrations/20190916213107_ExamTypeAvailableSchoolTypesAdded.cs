namespace TestOkur.WebApi.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ExamTypeAvailableSchoolTypesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "available_for_high_school",
                table: "exam_types",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "available_for_primary_school",
                table: "exam_types",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "available_for_high_school",
                table: "exam_types");

            migrationBuilder.DropColumn(
                name: "available_for_primary_school",
                table: "exam_types");
        }
    }
}
