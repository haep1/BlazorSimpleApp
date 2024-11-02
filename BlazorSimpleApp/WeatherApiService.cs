using System.Text.Json;
using System.Text.Json.Nodes;
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
    
    public static async Task<string?> GetWeatherInterpretation(List<HourlyWeather> weather, string city)
    {
        string chat = $"Our location is: {city}\n";
        chat += $"The current datetime is: {DateTime.Now}\n";
        chat += "The weather for the next days is as follows:\n";
        foreach (var hourlyWeather in weather)
        {
            chat += $"{hourlyWeather.Hour.ToString("HH:mm")}: {hourlyWeather.Temperature}°C\n";
        }
        chat += "How do you describe the weather for the next days? What should I wear when I go outside?\nAnswer in german!";
        string uri = $"http://127.0.0.1:8000/openaichat?chat={chat}";
        HttpClient client = new HttpClient();
        var response = await client.GetAsync(uri);
        //read content as a json object
        JsonDocument content = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync()).ConfigureAwait(false);
        string? responseText = content.RootElement.GetProperty("content").GetString();
        if (responseText != null)
        {
            responseText = Markdig.Markdown.ToHtml(responseText);
        }
        return responseText;
    }
}