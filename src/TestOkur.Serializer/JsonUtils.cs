namespace TestOkur.Serializer
{
    using System.IO;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    public static class JsonUtils
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };

        public static async ValueTask<T> DeserializerFromHttpContent<T>(
            HttpContent content,
            CancellationToken cancellationToken = default)
        {
            await using var utf8Json = await content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<T>(utf8Json, Options, cancellationToken);
        }

        public static async Task<T> DeserializeFromFileAsync<T>(string path, CancellationToken cancellationToken = default)
        {
            await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return await JsonSerializer.DeserializeAsync<T>(
                stream,
                Options,
                cancellationToken);
        }

        public static string Serialize<T>(T obj) => JsonSerializer.Serialize<T>(obj, Options);
    }
}
