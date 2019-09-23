using Microsoft.EntityFrameworkCore.Migrations;

namespace TestOkur.WebApi.Migrations
{
    public partial class SrcFieldsAddedToOpticalFormTextLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "course_name_x",
                table: "optical_form_text_locations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "course_name_y",
                table: "optical_form_text_locations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "title1_x",
                table: "optical_form_text_locations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "title1_y",
                table: "optical_form_text_locations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "title2_x",
                table: "optical_form_text_locations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "title2_y",
                table: "optical_form_text_locations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "course_name_x",
                table: "optical_form_text_locations");

            migrationBuilder.DropColumn(
                name: "course_name_y",
                table: "optical_form_text_locations");

            migrationBuilder.DropColumn(
                name: "title1_x",
                table: "optical_form_text_locations");

            migrationBuilder.DropColumn(
                name: "title1_y",
                table: "optical_form_text_locations");

            migrationBuilder.DropColumn(
                name: "title2_x",
                table: "optical_form_text_locations");

            migrationBuilder.DropColumn(
                name: "title2_y",
                table: "optical_form_text_locations");
        }
    }
}
