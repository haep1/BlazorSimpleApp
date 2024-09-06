using System.Text.Json.Serialization;

public class HourlyWeather
{
    [JsonPropertyName("date")]
    public DateTime Hour { get; set; }
    
    [JsonPropertyName("temperature_2m")]
    public double Temperature { get; set; }
}

