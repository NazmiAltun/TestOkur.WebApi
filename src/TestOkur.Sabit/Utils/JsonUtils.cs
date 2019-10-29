namespace TestOkur.Sabit.Utils
{
    using System.IO;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    public static class JsonUtils
    {
        public static async Task<T> ReadAsync<T>(string fileName, CancellationToken cancellationToken = default)
        {
            var path = Path.Join("Data", fileName);

            await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return await JsonSerializer.DeserializeAsync<T>(
                stream,
                DefaultJsonSerializerSettings.Instance,
                cancellationToken);
        }
    }
}
