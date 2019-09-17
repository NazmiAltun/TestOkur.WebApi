namespace TestOkur.WebApi.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class SmsGroupdAndContactAuditable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "created_by",
                table: "sms_groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_on_utc",
                table: "sms_groups",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "updated_by",
                table: "sms_groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_on_utc",
                table: "sms_groups",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "created_by",
                table: "contacts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_on_utc",
                table: "contacts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "updated_by",
                table: "contacts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_on_utc",
                table: "contacts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                table: "sms_groups");

            migrationBuilder.DropColumn(
                name: "created_on_utc",
                table: "sms_groups");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "sms_groups");

            migrationBuilder.DropColumn(
                name: "updated_on_utc",
                table: "sms_groups");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "created_on_utc",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "updated_on_utc",
                table: "contacts");
        }
    }
}
