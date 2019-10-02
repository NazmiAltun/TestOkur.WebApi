namespace TestOkur.WebApi.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class SharedFlagAddedForSharableDomainModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "shared",
                table: "units",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "shared",
                table: "subjects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "shared",
                table: "exams",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "shared",
                table: "units");

            migrationBuilder.DropColumn(
                name: "shared",
                table: "subjects");

            migrationBuilder.DropColumn(
                name: "shared",
                table: "exams");
        }
    }
}
