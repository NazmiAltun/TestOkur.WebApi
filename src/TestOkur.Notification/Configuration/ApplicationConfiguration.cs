namespace TestOkur.Notification.Configuration
{
    using System.ComponentModel.DataAnnotations;

    public class ApplicationConfiguration
    {
        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public string Database { get; set; }

        [Required]
        public int RemainderDays { get; set; }

        [Required]
        public string ProductOwnersEmails { get; set; }

        [Required]
        public string SystemAdminEmails { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string SeqUrl { get; set; }
    }
}
