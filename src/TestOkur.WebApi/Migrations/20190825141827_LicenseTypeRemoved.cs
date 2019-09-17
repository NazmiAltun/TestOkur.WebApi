namespace TestOkur.WebApi.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class LicenseTypeRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "license_types");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "license_types",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    can_scan = table.Column<bool>(nullable: false),
                    max_allowed_device_count = table.Column<int>(nullable: false),
                    max_allowed_record_count = table.Column<int>(nullable: false),
                    name_value = table.Column<string>(maxLength: 150, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_license_types", x => x.id);
                });
        }
    }
}
