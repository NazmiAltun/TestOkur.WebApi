namespace TestOkur.WebApi.Migrations
{
	using System;
	using Microsoft.EntityFrameworkCore.Migrations;
	using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

	public partial class SmsGroupsRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contacts_sms_groups_sms_group_id",
                table: "contacts");

            migrationBuilder.DropTable(
                name: "sms_groups");

            migrationBuilder.DropIndex(
                name: "ix_contacts_sms_group_id",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "sms_group_id",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "use_for_sms",
                table: "contacts");

            migrationBuilder.AddColumn<string>(
                name: "labels",
                table: "contacts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "labels",
                table: "contacts");

            migrationBuilder.AddColumn<long>(
                name: "sms_group_id",
                table: "contacts",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "use_for_sms",
                table: "contacts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "sms_groups",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                    name_value = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sms_groups", x => x.id);
                });

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
    }
}
