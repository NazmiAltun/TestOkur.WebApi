using Microsoft.EntityFrameworkCore.Migrations;

namespace TestOkur.WebApi.Migrations
{
    public partial class SubjectNameLengthIncreased : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name_value",
                table: "subjects",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 150);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name_value",
                table: "subjects",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 300);
        }
    }
}
