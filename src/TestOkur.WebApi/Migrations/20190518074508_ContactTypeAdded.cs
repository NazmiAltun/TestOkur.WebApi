namespace TestOkur.WebApi.Migrations
{
	using Microsoft.EntityFrameworkCore.Migrations;

	public partial class ContactTypeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "school_types",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AddColumn<long>(
                name: "contact_type_id",
                table: "contacts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "first_name_value",
                table: "contacts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_name_value",
                table: "contacts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "contact_types",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    name = table.Column<string>(maxLength: 20, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contact_types", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_contacts_contact_type_id",
                table: "contacts",
                column: "contact_type_id");

            migrationBuilder.AddForeignKey(
                name: "fk_contacts_contact_types_contact_type_id",
                table: "contacts",
                column: "contact_type_id",
                principalTable: "contact_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contacts_contact_types_contact_type_id",
                table: "contacts");

            migrationBuilder.DropTable(
                name: "contact_types");

            migrationBuilder.DropIndex(
                name: "ix_contacts_contact_type_id",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "contact_type_id",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "first_name_value",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "last_name_value",
                table: "contacts");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "school_types",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30);
        }
    }
}
