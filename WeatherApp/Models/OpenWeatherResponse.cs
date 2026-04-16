using Newtonsoft.Json;
using System.Collections.Generic;

namespace WeatherApp.Models
{
    public class OpenWeatherResponse
    {
        [JsonProperty("coord")]
        public Coord Coord { get; set; }

        [JsonProperty("weather")]
        public List<Weather> Weather { get; set; }

        [JsonProperty("main")]
        public Main Main { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }

        [JsonProperty("dt")]
        public long Dt { get; set; }

        [JsonProperty("sys")]
        public Sys Sys { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Coord
    {
        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }
    }

    public class Weather
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("main")]
        public string Main { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

    public class Main
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("feels_like")]
        public double FeelsLike { get; set; }

        [JsonProperty("temp_min")]
        public double TempMin { get; set; }

        [JsonProperty("temp_max")]
        public double TempMax { get; set; }

        [JsonProperty("pressure")]
        public int Pressure { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }
    }

    public class Wind
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }
    }

    public class Sys
    {
        [JsonProperty("country")]
        public string Country { get; set; }
    }

    public class OpenWeatherForecastResponse
    {
        [JsonProperty("list")]
        public List<ForecastItem> List { get; set; }

        [JsonProperty("city")]
        public City City { get; set; }
    }

    public class ForecastItem
    {
        [JsonProperty("dt")]
        public long Dt { get; set; }

        [JsonProperty("main")]
        public Main Main { get; set; }

        [JsonProperty("weather")]
        public List<Weather> Weather { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }
    }

    public class City
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
