namespace TestOkur.Domain.Model.OpticalFormModel
{
    public class OpticalFormTextLocationBuilder
    {
        private Location _name = Location.Empty;
        private Location _surname = Location.Empty;
        private Location _class = Location.Empty;
        private Location _studentNo = Location.Empty;
        private Location _examName = Location.Empty;
        private Location _studentNoFillingPart = Location.Empty;
        private Location _title1 = Location.Empty;
        private Location _title2 = Location.Empty;
        private Location _courseName = Location.Empty;
        private Location _citizenshipIdentity = Location.Empty;

        public OpticalFormTextLocation Build()
        {
            return new OpticalFormTextLocation(
                _name,
                _surname,
                _class,
                _studentNo,
                _examName,
                _studentNoFillingPart,
                _title1,
                _title2,
                _courseName,
                _citizenshipIdentity);
        }

        public OpticalFormTextLocationBuilder SetStudentNoFillingPartLocation(int x, int y)
        {
            _studentNoFillingPart = Location.From(x, y);
            return this;
        }

        public OpticalFormTextLocationBuilder SetExamNameLocation(int x, int y)
        {
            _examName = Location.From(x, y);
            return this;
        }

        public OpticalFormTextLocationBuilder SetStudentNoLocation(int x, int y)
        {
            _studentNo = Location.From(x, y);
            return this;
        }

        public OpticalFormTextLocationBuilder SetClassLocation(int x, int y)
        {
            _class = Location.From(x, y);
            return this;
        }

        public OpticalFormTextLocationBuilder SetSurnameLocation(int x, int y)
        {
            _surname = Location.From(x, y);
            return this;
        }

        public OpticalFormTextLocationBuilder SetNameLocation(int x, int y)
        {
            _name = Location.From(x, y);
            return this;
        }

        public OpticalFormTextLocationBuilder SetTitle1Location(int x, int y)
        {
            _title1 = Location.From(x, y);
            return this;
        }

        public OpticalFormTextLocationBuilder SetTitle2Location(int x, int y)
        {
            _title2 = Location.From(x, y);
            return this;
        }

        public OpticalFormTextLocationBuilder SetCourseNameLocation(int x, int y)
        {
            _courseName = Location.From(x, y);
            return this;
        }

        public OpticalFormTextLocationBuilder SetCitizenshipIdentityLocation(int x, int y)
        {
            _citizenshipIdentity = Location.From(x, y);
            return this;
        }
    }
}
