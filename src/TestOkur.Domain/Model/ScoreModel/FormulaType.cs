namespace TestOkur.Domain.Model.ScoreModel
{
	using TestOkur.Domain.SeedWork;

	public class FormulaType : Enumeration
    {
        public static readonly FormulaType TytAyt = new FormulaType(1, "TYT + AYT");
        public static readonly FormulaType Scholarship = new FormulaType(2, "Bursluluk");
        public static readonly FormulaType Trial = new FormulaType(3, "Deneme");

        public FormulaType(int id, string name)
            : base(id, name)
        {
        }

        public FormulaType()
        {
        }
    }
}
