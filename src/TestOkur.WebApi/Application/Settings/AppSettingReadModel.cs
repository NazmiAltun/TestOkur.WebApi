namespace TestOkur.WebApi.Application.Settings
{
    using System;

    public class AppSettingReadModel
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedOnUTC { get; set; }

        public DateTime UpdateOnUTC { get; set; }
    }
}
