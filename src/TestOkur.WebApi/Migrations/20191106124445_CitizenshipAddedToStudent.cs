using Microsoft.EntityFrameworkCore.Migrations;

namespace TestOkur.WebApi.Migrations
{
    public partial class CitizenshipAddedToStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "citizenship_identity",
                table: "students",
                maxLength: 12,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "citizenship_identity",
                table: "students");
        }
    }
}
