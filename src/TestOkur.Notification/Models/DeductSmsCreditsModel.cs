namespace TestOkur.Notification.Models
{
	using System;

	public class DeductSmsCreditsModel
    {
        public Guid Id { get; set; }

        public int UserId { get; set; }

        public string SmsBody { get; set; }
    }
}
