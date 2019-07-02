namespace TestOkur.Domain.Model.OpticalFormModel
{
	using System.Collections.Generic;
	using TestOkur.Domain.SeedWork;

	public class OpticalFormType : Entity
	{
		private readonly List<FormLessonSection> _formLessonSections;
		private readonly List<OpticalFormDefinition> _opticalFormDefinitions;

		public OpticalFormType(
			Name name,
			string code,
			string configurationFile,
			SchoolType schoolType,
			IEnumerable<FormLessonSection> lessonSections,
			int maxQuestionCount)
			: this()
		{
			Name = name;
			Code = code;
			ConfigurationFile = configurationFile;
			SchoolType = schoolType;
			MaxQuestionCount = maxQuestionCount;

			if (lessonSections != null)
			{
				_formLessonSections.AddRange(lessonSections);
			}
		}

		protected OpticalFormType()
		{
			_formLessonSections = new List<FormLessonSection>();
			_opticalFormDefinitions = new List<OpticalFormDefinition>();
		}

		public Name Name { get; private set; }

		public SchoolType SchoolType { get; private set; }

		public int MaxQuestionCount { get; private set; }

		public string Code { get; private set; }

		public string ConfigurationFile { get; private set; }

		public IEnumerable<OpticalFormDefinition> OpticalFormDefinitions
			=> _opticalFormDefinitions.AsReadOnly();

		public IEnumerable<FormLessonSection> FormLessonSections
			=> _formLessonSections.AsReadOnly();

		public void AddOpticalFormDefinition(OpticalFormDefinition formDefinition)
		{
			_opticalFormDefinitions.Add(formDefinition);
		}
	}
}
