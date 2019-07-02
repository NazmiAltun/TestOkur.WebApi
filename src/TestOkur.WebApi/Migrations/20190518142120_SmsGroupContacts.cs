namespace TestOkur.WebApi.Migrations
{
	using Microsoft.EntityFrameworkCore.Migrations;

	public partial class SmsGroupContacts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "sms_group_id",
                table: "contacts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_contacts_sms_group_id",
                table: "contacts",
                column: "sms_group_id");

            migrationBuilder.AddForeignKey(
                name: "fk_contacts_sms_groups_sms_group_id",
                table: "contacts",
                column: "sms_group_id",
                principalTable: "sms_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contacts_sms_groups_sms_group_id",
                table: "contacts");

            migrationBuilder.DropIndex(
                name: "ix_contacts_sms_group_id",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "sms_group_id",
                table: "contacts");
        }
    }
}
