namespace TestOkur.Notification.Configuration
{
    using System.ComponentModel.DataAnnotations;

    public class SmsConfiguration
	{
		[Required]
		public string ServiceUrl { get; set; }

		[Required]
		public string User { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
