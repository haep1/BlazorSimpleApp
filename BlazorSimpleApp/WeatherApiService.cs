using System.Text.Json;

namespace BlazorSimpleApp;

public static class WeatherApiService
{
    public static async void GetWeather(double longigude, double latitude)
    {
        // Call the API to get the weather
        HttpClient client = new HttpClient();
        //var response = await client.GetAsync($"http://127.0.0.1:8000/weather/{longigude}/{latitude}");
        var response = await client.GetAsync($"http://127.0.0.1:8000/sampleWeather");
        var content = await response.Content.ReadAsStringAsync();
        content = content.Replace("\\", "");
        content = content.Substring(1, content.Length - 2);
        List<HourlyWeather>? weather = JsonSerializer.Deserialize<List<HourlyWeather>>(content);
    }
}