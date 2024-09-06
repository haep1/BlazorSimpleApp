using System.Text.Json;
using BlazorSimpleApp.GeoLocation;

namespace BlazorSimpleApp;

public static class WeatherApiService
{
    public static async Task<List<HourlyWeather>?> GetWeather(float latitude, float longitude)
    {
        // Call the API to get the weather
        HttpClient client = new HttpClient();
        string uri = $"http://127.0.0.1:8000/weather/{latitude.ToString("F", 
                System.Globalization.CultureInfo.InvariantCulture)}/{longitude.ToString("F", 
                System.Globalization.CultureInfo.InvariantCulture)}";
        var response = await client.GetAsync(uri);
        var content = await response.Content.ReadAsStringAsync();
        content = content.Replace("\\", "");
        content = content.Substring(1, content.Length - 2);
        List<HourlyWeather>? weather = JsonSerializer.Deserialize<List<HourlyWeather>>(content);
        return weather;
    }
}