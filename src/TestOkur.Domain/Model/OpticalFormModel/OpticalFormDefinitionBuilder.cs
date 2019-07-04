namespace TestOkur.Domain.Model.OpticalFormModel
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;

	public class OpticalFormDefinitionBuilder
    {
        private readonly string _name;
        private readonly IList<OpticalFormTextLocation> _textLocations;

        private int _studentNoFillWidth;
        private int _studentNoXInterval;
        private int _studentNoYInterval;
        private bool _hasBoxForStudentNumber;
        private string _description;
        private string _fileName;
        private Direction _textDirection;
        private Direction _studentNumberFillDirection;
        private SchoolType _schoolType;

        public OpticalFormDefinitionBuilder(string name)
        {
            _name = name;
            _textLocations = new List<OpticalFormTextLocation>();
        }

        private string SubFolder =>
	        _schoolType == SchoolType.PrimaryAndSecondary
		        ? "PrimarySchool"
		        : "HighSchool";

        public OpticalFormDefinition Build()
        {
	        return new OpticalFormDefinition(
                _name,
                _studentNoFillWidth,
                _studentNoXInterval,
                _studentNoYInterval,
                _hasBoxForStudentNumber,
                _description,
                $@"OpticalForms\{SubFolder}\{_fileName}",
                _textDirection,
                _studentNumberFillDirection,
                _schoolType,
                new ReadOnlyCollection<OpticalFormTextLocation>(_textLocations));
        }

        public OpticalFormDefinitionBuilder AddTextLocation(OpticalFormTextLocation location)
        {
            _textLocations.Add(location);

            return this;
        }

        public OpticalFormDefinitionBuilder PrimarySchool()
        {
            _schoolType = SchoolType.PrimaryAndSecondary;
            return this;
        }

        public OpticalFormDefinitionBuilder HighSchool()
        {
            _schoolType = SchoolType.High;
            return this;
        }

        public OpticalFormDefinitionBuilder SetStudentNumberFillDirection(Direction value)
        {
            _studentNumberFillDirection = value;
            return this;
        }

        public OpticalFormDefinitionBuilder SetTextDirection(Direction value)
        {
            _textDirection = value;
            return this;
        }

        public OpticalFormDefinitionBuilder SetStudentNoXInterval(int value)
        {
            _studentNoXInterval = value;
            return this;
        }

        public OpticalFormDefinitionBuilder SetStudentNoFillWidth(int value)
        {
            _studentNoFillWidth = value;
            return this;
        }

        public OpticalFormDefinitionBuilder SetStudentNoYInterval(int value)
        {
            _studentNoYInterval = value;
            return this;
        }

        public OpticalFormDefinitionBuilder HasBoxForStudentNumber()
        {
            _hasBoxForStudentNumber = true;
            return this;
        }

        public OpticalFormDefinitionBuilder SetDescription(string value)
        {
            _description = value;
            return this;
        }

        public OpticalFormDefinitionBuilder SetFilename(string value)
        {
            _fileName = value;
            return this;
        }
    }
}
