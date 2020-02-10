namespace TestOkur.Common.Configuration
{
    using System.ComponentModel.DataAnnotations;

    public class RabbitMqConfiguration
    {
        [Required]
        public string Uri { get; set; }

        [Required]
        public string Vhost { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
