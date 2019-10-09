namespace TestOkur.Report.Domain
{
    public class SchoolResultSection
    {
        public string LessonName { get; set; }

        public int QuestionCount { get; set; }

        public float CorrectCount { get; set; }

        public float WrongCount { get; set; }

        public float EmptyCount { get; set; }

        public float Net { get; set; }

        public float SuccessPercent { get; set; }

        public float DistrictAverageNet { get; set; }

        public float CityAverageNet { get; set; }

        public float GeneralAverageNet { get; set; }

        public int DistrictOrder { get; set; }

        public int CityOrder { get; set; }

        public int GeneralOrder { get; set; }
    }
}