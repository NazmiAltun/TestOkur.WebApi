namespace TestOkur.Report.Domain.Statistics
{
    using System.Collections.Generic;

    public class SectionAverage
    {
        public float GeneralSuccessPercent { get; set; }

        public Dictionary<int, float> CitySuccessPercents { get; set; }

        public Dictionary<int, float> DistrictSuccessPercents { get; set; }

        public Dictionary<int, float> SchoolSuccessPercents { get; set; }

        public Dictionary<int, float> ClassroomSuccessPercents { get; set; }

        public float GeneralNet { get; set; }

        public Dictionary<int, float> CityNets { get; set; }

        public Dictionary<int, float> DistrictNets { get; set; }

        public Dictionary<int, float> SchoolNets { get; set; }

        public Dictionary<int, float> ClassroomNets { get; set; }

        public Dictionary<int, float> SchoolCorrectCounts { get; set; }

        public Dictionary<int, float> SchoolEmptyCounts { get; set; }

        public Dictionary<int, float> SchoolWrongCounts { get; set; }
    }
}