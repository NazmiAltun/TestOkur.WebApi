namespace TestOkur.WebApi.Migrations
{
	using System.Diagnostics.CodeAnalysis;

	using Microsoft.EntityFrameworkCore.Migrations;

	[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1122:UseStringEmptyForEmptyStrings", Justification = "Reviewed.")]
	public partial class ExamScoreFormula : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "exam_id",
                table: "score_formulas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "discriminator",
                table: "score_formulas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_score_formulas_exam_id",
                table: "score_formulas",
                column: "exam_id");

            migrationBuilder.AddForeignKey(
                name: "fk_score_formulas_exams_exam_id",
                table: "score_formulas",
                column: "exam_id",
                principalTable: "exams",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_score_formulas_exams_exam_id",
                table: "score_formulas");

            migrationBuilder.DropIndex(
                name: "ix_score_formulas_exam_id",
                table: "score_formulas");

            migrationBuilder.DropColumn(
                name: "exam_id",
                table: "score_formulas");

            migrationBuilder.DropColumn(
                name: "discriminator",
                table: "score_formulas");
        }
    }
}
