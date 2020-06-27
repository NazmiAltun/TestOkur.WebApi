namespace TestOkur.WebApi.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class SinglePagePerStudentAddedToOpticalFormDefinition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "single_page_per_student",
                table: "optical_form_definitions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "single_page_per_student",
                table: "optical_form_definitions");
        }
    }
}
