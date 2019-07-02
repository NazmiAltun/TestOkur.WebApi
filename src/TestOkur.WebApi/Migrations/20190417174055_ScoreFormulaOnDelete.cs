namespace TestOkur.WebApi.Migrations
{
	using Microsoft.EntityFrameworkCore.Migrations;

	public partial class ScoreFormulaOnDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_lesson_coefficients_score_formulas_score_formula_id",
                table: "lesson_coefficients");

            migrationBuilder.AddForeignKey(
                name: "fk_lesson_coefficients_score_formulas_score_formula_id",
                table: "lesson_coefficients",
                column: "score_formula_id",
                principalTable: "score_formulas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_lesson_coefficients_score_formulas_score_formula_id",
                table: "lesson_coefficients");

            migrationBuilder.AddForeignKey(
                name: "fk_lesson_coefficients_score_formulas_score_formula_id",
                table: "lesson_coefficients",
                column: "score_formula_id",
                principalTable: "score_formulas",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
