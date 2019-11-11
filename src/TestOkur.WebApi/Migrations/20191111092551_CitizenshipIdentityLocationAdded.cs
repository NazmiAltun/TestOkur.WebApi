using Microsoft.EntityFrameworkCore.Migrations;

namespace TestOkur.WebApi.Migrations
{
    public partial class CitizenshipIdentityLocationAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "citizenship_identity_x",
                table: "optical_form_text_locations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "citizenship_identity_y",
                table: "optical_form_text_locations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "citizenship_identity_x",
                table: "optical_form_text_locations");

            migrationBuilder.DropColumn(
                name: "citizenship_identity_y",
                table: "optical_form_text_locations");
        }
    }
}
