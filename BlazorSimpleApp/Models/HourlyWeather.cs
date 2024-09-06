using System.Text.Json.Serialization;

public class HourlyWeather
{
    [JsonPropertyName("date")]
    public string Hour { get; set; }
    
    [JsonPropertyName("temperature_2m")]
    public double Temperature { get; set; }
}

