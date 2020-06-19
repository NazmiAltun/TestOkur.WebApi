namespace TestOkur.WebApi.Configuration
{
    using System.ComponentModel.DataAnnotations;

    public class ApplicationConfiguration
    {
        [Required]
        public string Postgres { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string SeqUrl { get; set; }

        [Required]
        public string CaptchaServiceUrl { get; set; }

        [Required]
        public string SabitApiUrl { get; set; }
    }
}
