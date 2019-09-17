namespace TestOkur.Domain.Model.ExamModel
{
    using System.Collections.Generic;
    using TestOkur.Domain.Model.OpticalFormModel;
    using TestOkur.Domain.SeedWork;

    public class ExamType : Entity, IAuditable
	{
		private readonly List<ExamTypeOpticalFormType> _examTypeOpticalFormTypes;

		public ExamType(
			Name name,
			IncorrectEliminationRate defaultIncorrectEliminationRate,
			bool availableForPrimarySchool,
			bool availableForHighSchool,
			int order)
		: this()
		{
			Name = name;
			DefaultIncorrectEliminationRate = defaultIncorrectEliminationRate;
			AvailableForPrimarySchool = availableForPrimarySchool;
			AvailableForHighSchool = availableForHighSchool;
			Order = order;
		}

		protected ExamType()
		{
			_examTypeOpticalFormTypes = new List<ExamTypeOpticalFormType>();
		}

		public Name Name { get; private set; }

		public IncorrectEliminationRate DefaultIncorrectEliminationRate { get; private set; }

		public int Order { get; private set; }

		public bool AvailableForPrimarySchool { get; private set; }

		public bool AvailableForHighSchool { get; private set; }

		public IEnumerable<ExamTypeOpticalFormType> ExamTypeOpticalFormTypes
			=> _examTypeOpticalFormTypes.AsReadOnly();

		public void AddFormType(OpticalFormType formType)
		{
			_examTypeOpticalFormTypes.Add(new ExamTypeOpticalFormType(formType));
		}
	}
}
