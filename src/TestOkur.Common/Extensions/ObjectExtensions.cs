namespace System
{
	using System.Net.Http;
	using System.Text;
	using Newtonsoft.Json;

	public static class ObjectExtensions
    {
        public static StringContent ToJsonContent<TModel>(this TModel obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}
