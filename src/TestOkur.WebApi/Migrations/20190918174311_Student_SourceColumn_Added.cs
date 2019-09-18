namespace TestOkur.WebApi.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Student_SourceColumn_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "source",
                table: "students",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "source",
                table: "students");
        }
    }
}
