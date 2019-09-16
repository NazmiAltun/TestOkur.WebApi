namespace TestOkur.Domain.Model.ExamModel
{
    using System.Collections.Generic;
    using TestOkur.Domain.Model.OpticalFormModel;

    public sealed class ExamTypeBuilder
    {
        private readonly string _name;
        private readonly List<OpticalFormType> _formTypes;
        private int _defaultIncorrectCorrectEliminationValue;
        private int _order;
        private bool _availableForHighSchool;
        private bool _availableForPrimarySchool;

        public ExamTypeBuilder(string name)
        {
            _name = name;
            _formTypes = new List<OpticalFormType>();
        }

        public ExamTypeBuilder WithDefaultIncorrectCorrectEliminationValue(int value)
        {
            _defaultIncorrectCorrectEliminationValue = value;

            return this;
        }

        public ExamTypeBuilder WithOrder(int value)
        {
            _order = value;

            return this;
        }

        public ExamTypeBuilder AvailableForAllSchoolTypes()
        {
            _availableForHighSchool = true;
            _availableForPrimarySchool = true;

            return this;
        }

        public ExamTypeBuilder AvailableForHighSchool()
        {
            _availableForHighSchool = true;

            return this;
        }

        public ExamTypeBuilder AvailableForPrimarySchool()
        {
            _availableForPrimarySchool = true;

            return this;
        }

        public ExamTypeBuilder AddFormType(OpticalFormType formType)
        {
            _formTypes.Add(formType);
            return this;
        }

        public ExamType Build()
        {
            var examType = new ExamType(
                _name,
                _defaultIncorrectCorrectEliminationValue,
                _availableForPrimarySchool,
                _availableForHighSchool,
                _order);

            foreach (var formType in _formTypes)
            {
                examType.AddFormType(formType);
            }

            return examType;
        }
    }
}
