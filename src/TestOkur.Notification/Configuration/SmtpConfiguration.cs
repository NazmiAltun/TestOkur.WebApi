namespace TestOkur.Notification.Configuration
{
    using System.ComponentModel.DataAnnotations;

    public class SmtpConfiguration
    {
        [Required]
        public string Host { get; set; }

        [Required]
        public int Port { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FromName { get; set; }

        [Required]
        public bool UseDefaultCredentials { get; set; }

        [Required]
        public bool EnableSsl { get; set; }
    }
}
