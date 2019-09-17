namespace TestOkur.Common.Configuration
{
    using System.ComponentModel.DataAnnotations;

    public class OAuthConfiguration
    {
		[Required]
		public string Authority { get; set; }

		[Required]
		public bool RequireHttpsMetadata { get; set; }

		[Required]
		public string ApiName { get; set; }

		[Required]
		public string PrivateClientSecret { get; set; }
    }
}
