namespace TestOkur.Notification.Configuration
{
    using System.ComponentModel.DataAnnotations;

    public class HangfireConfiguration
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
