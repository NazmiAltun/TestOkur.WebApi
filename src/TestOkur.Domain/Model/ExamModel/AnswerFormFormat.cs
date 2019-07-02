namespace TestOkur.Domain.Model.ExamModel
{
	using TestOkur.Domain.SeedWork;

	public class AnswerFormFormat : Enumeration
	{
		public static readonly AnswerFormFormat Separate = new AnswerFormFormat(1, "Her kitapçık için cevapları ayrı ayrı gir");
		public static readonly AnswerFormFormat QuestionNoMatch = new AnswerFormFormat(2, "İlk kitapçığa göre soru karşılıklarını gir");

		public AnswerFormFormat(int id, string name)
			: base(id, name)
		{
		}

		protected AnswerFormFormat()
		{
		}
	}
}
