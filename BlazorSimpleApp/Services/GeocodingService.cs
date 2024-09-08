using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorSimpleApp.Services
{
    public class GeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeocodingService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<string?> GetNearestCityAsync(double latitude, double longitude)
        {
            var url = $"https://api.opencagedata.com/geocode/v1/json?q={latitude}+{longitude}&key={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<OpenCageResponse>(json,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return data?.Results?.FirstOrDefault()?.Components?.City;
        }

        private class OpenCageResponse
        {
            public List<Result>? Results { get; set; }
        }

        private class Result
        {
            public Components? Components { get; set; }
        }

        private class Components
        {
            [JsonPropertyName("_normalized_city")]
            public string? City { get; set; }
        }
    }
}