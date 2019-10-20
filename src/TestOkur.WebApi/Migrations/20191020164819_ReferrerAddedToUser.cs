namespace TestOkur.WebApi.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ReferrerAddedToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "referrer",
                table: "users",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "referrer",
                table: "users");
        }
    }
}
