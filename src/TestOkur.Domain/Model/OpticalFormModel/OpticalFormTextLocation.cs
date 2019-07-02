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
            Location studentNoFillingPart)
        {
            Name = name;
            Surname = surname;
            Class = @class;
            StudentNo = studentNo;
            ExamName = examName;
            StudentNoFillingPart = studentNoFillingPart;
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
    }
}
