namespace TestOkur.Domain.Model.OpticalFormModel
{
    using TestOkur.Domain.SeedWork;

    public class OpticalFormTextLocation : Entity
    {
        public OpticalFormTextLocation(
            Location name,
            Location surname,
            Location @class,
            Location studentNo,
            Location examName,
            Location studentNoFillingPart,
            Location title1,
            Location title2,
            Location courseName,
            Location citizenshipIdentity)
        {
            Name = name;
            Surname = surname;
            Class = @class;
            StudentNo = studentNo;
            ExamName = examName;
            StudentNoFillingPart = studentNoFillingPart;
            Title1 = title1;
            Title2 = title2;
            CourseName = courseName;
            CitizenshipIdentity = citizenshipIdentity;
        }

        protected OpticalFormTextLocation()
        {
        }

        public Location Name { get; private set; }

        public Location Surname { get; private set; }

        public Location Class { get; private set; }

        public Location StudentNo { get; private set; }

        public Location ExamName { get; private set; }

        public Location StudentNoFillingPart { get; private set; }

        public Location Title1 { get; private set; }

        public Location Title2 { get; private set; }

        public Location CourseName { get; private set; }

        public Location CitizenshipIdentity { get; private set; }
    }
}
