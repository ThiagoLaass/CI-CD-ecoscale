using System.Text;
using System.Text.Json;

namespace EcoScale.src.Services.Helpers
{
    public class Api()
    {
        public StringContent GetPayload<T>(T payload)
        {
            return new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );
        }

        public HttpClient GetClient()
        {
            return new HttpClient();
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T payload)
        {
            var client = GetClient();
            var content = GetPayload(payload);
            return await client.PostAsync(uri, content);
        }

        public async Task<HttpResponseMessage> GetAsync(string uri)
        {
            var client = GetClient();
            return await client.GetAsync(uri);
        }
    }
}