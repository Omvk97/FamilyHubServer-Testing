using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace API.IntegrationTests.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync<TModel>(this HttpClient client, string requestUrl, TModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            return await client.PostAsync(requestUrl, stringContent);
        }
    }
}
