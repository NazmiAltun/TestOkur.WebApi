namespace TestOkur.Report.Domain.Optic
{
    public class StudentOpticalForm : OpticalForm
    {
        public int StudentId { get; set; }

        public ScanResult[] ScanResults { get; set; }
    }
}
