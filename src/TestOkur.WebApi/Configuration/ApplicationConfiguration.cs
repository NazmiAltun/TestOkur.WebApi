namespace TestOkur.WebApi.Configuration
{
    using System.ComponentModel.DataAnnotations;

    public class ApplicationConfiguration
    {
        [Required]
        public string Postgres { get; set; }

        [Required]
        public string Key { get; set; }
    }
}
