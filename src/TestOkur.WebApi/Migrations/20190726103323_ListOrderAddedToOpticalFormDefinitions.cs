using Microsoft.EntityFrameworkCore.Migrations;

namespace TestOkur.WebApi.Migrations
{
    public partial class ListOrderAddedToOpticalFormDefinitions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "list_order",
                table: "optical_form_definitions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "list_order",
                table: "optical_form_definitions");
        }
    }
}
