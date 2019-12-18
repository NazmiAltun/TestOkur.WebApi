namespace TestOkur.Report.Configuration
{
    using System.ComponentModel.DataAnnotations;

    public class ReportConfiguration
    {
        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public string Database { get; set; }

        [Required]
        public string Key { get; set; }
    }
}
