namespace TestOkur.Serializer
{
    using SpanJson;
    using SpanJson.Resolvers;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public static class JsonUtils
    {
        public static async ValueTask<T> DeserializerFromHttpContentAsyncWithCamelCaseResolver<T>(
            HttpContent content,
            CancellationToken cancellationToken = default)
        {
            await using var stream = await content.ReadAsStreamAsync();

            return await JsonSerializer.Generic.Utf8.DeserializeAsync<T, ExcludeNullsCamelCaseResolver<byte>>(
                stream,
                cancellationToken);
        }

        public static async ValueTask<T> DeserializerFromHttpContentAsyncWithOriginalCaseResolver<T>(
            HttpContent content,
            CancellationToken cancellationToken = default)
        {
            await using var stream = await content.ReadAsStreamAsync();

            return await JsonSerializer.Generic.Utf8.DeserializeAsync<T, ExcludeNullsOriginalCaseResolver<byte>>(
                stream,
                cancellationToken);
        }


        public static async Task<T> DeserializeFromFileAsync<T>(string path, CancellationToken cancellationToken = default)
        {
            await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var result = await JsonSerializer.Generic.Utf8.DeserializeAsync<T, ExcludeNullsCamelCaseResolver<byte>>(
                stream,
                cancellationToken);

            return result;
        }

        public static string Serialize<T>(T obj)
        {
            var bytes = JsonSerializer.Generic.Utf8.Serialize<T>(obj);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
