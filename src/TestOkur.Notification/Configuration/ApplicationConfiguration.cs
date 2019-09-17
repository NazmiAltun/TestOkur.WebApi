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
	}
}
