using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EcoScale.src.Services.Helpers
{
    public class ApiHelper
    {
        private static readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        static ApiHelper()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(2)
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static StringContent GetPayload<T>(T payload)
        {
            var json = JsonSerializer.Serialize(payload, _jsonOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T payload)
        {
            using var content = GetPayload(payload);
            return await _httpClient.PostAsync(uri, content);
        }

        public async Task<HttpResponseMessage> GetAsync(string uri)
        {
            return await _httpClient.GetAsync(uri);
        }
    }
}
