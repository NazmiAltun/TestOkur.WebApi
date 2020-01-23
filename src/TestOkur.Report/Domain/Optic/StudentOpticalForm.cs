namespace TestOkur.Report.Domain.Optic
{
    public class StudentOpticalForm : OpticalForm
    {
        public int StudentId { get; set; }

        public int ClassroomId { get; set; }

        public int DistrictId { get; set; }

        public int CityId { get; set; }

        public ScanResult[] ScanResults { get; set; }
    }
}
