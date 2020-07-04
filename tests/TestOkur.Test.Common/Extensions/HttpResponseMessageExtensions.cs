namespace TestOkur.Test.Common.Extensions
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Serialization;

    public static class HttpResponseMessageExtensions
    {
        public static ValueTask<T> ReadAsync<T>(this HttpResponseMessage responseMessage)
        {
            return JsonUtils.DeserializerFromHttpContentAsync<T>(responseMessage.Content);
        }
    }
}
