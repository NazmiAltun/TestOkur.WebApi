namespace TestOkur.Sabit.Configuration
{
    using System.ComponentModel.DataAnnotations;

    public class ApplicationConfiguration
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string SeqUrl { get; set; }
    }
}
