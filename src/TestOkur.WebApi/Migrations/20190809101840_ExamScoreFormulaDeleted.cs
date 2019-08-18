namespace TestOkur.WebApi.Migrations
{
	using Microsoft.EntityFrameworkCore.Migrations;

	public partial class ExamScoreFormulaDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "exam_id",
                table: "score_formulas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "discriminator",
                table: "score_formulas",
                nullable: false,
                defaultValue: string.Empty);

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
    }
}
