namespace TestOkur.Common
{
    using System.Text.Json;

    public static class DefaultJsonSerializerSettings
    {
        public static readonly JsonSerializerOptions Instance = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }
}
