namespace TestOkur.WebApi.Application.Exam.Queries
{
	using System.Collections.Generic;
	using TestOkur.WebApi.Application.OpticalForm;

	public class ExamTypeReadModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DefaultIncorrectEliminationRate { get; set; }

        public List<OpticalFormTypeReadModel> OpticalFormTypes { get; set; }
            = new List<OpticalFormTypeReadModel>();
    }
}
