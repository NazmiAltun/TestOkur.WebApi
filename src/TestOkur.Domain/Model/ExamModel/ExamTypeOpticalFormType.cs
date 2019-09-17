namespace TestOkur.Domain.Model.ExamModel
{
    using TestOkur.Domain.Model.OpticalFormModel;
    using TestOkur.Domain.SeedWork;

    public class ExamTypeOpticalFormType : Entity
	{
		public ExamTypeOpticalFormType(OpticalFormType opticalFormType)
		{
			OpticalFormType = opticalFormType;
		}

		protected ExamTypeOpticalFormType()
		{
		}

		public OpticalFormType OpticalFormType { get; private set; }
	}
}
