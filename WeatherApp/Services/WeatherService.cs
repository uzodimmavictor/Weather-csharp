using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string BASE_URL = "https://api.openweathermap.org/data/2.5";

        public WeatherService()
        {
            _httpClient = new HttpClient();
            _apiKey = LoadApiKey();
        }

        private string LoadApiKey()
        {
            string? apiKey = Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY");

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.local.json");

                if (File.Exists(configPath))
                {
                    string configJson = File.ReadAllText(configPath);
                    using JsonDocument doc = JsonDocument.Parse(configJson);

                    if (doc.RootElement.TryGetProperty("OpenWeatherApiKey", out JsonElement keyElement))
                    {
                        apiKey = keyElement.GetString();
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "Clé API manquante. Définissez OPENWEATHER_API_KEY ou créez appsettings.local.json avec OpenWeatherApiKey.");
            }

            return apiKey;
        }

        public async Task<WeatherData> GetCurrentWeatherAsync(string cityName)
        {
            try
            {
                string url = $"{BASE_URL}/weather?q={cityName}&appid={_apiKey}&units=metric&lang=fr";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Erreur lors de la récupération de la météo: {response.StatusCode}");
                }

                string json = await response.Content.ReadAsStringAsync();
                OpenWeatherResponse apiResponse = JsonConvert.DeserializeObject<OpenWeatherResponse>(json);

                return new WeatherData
                {
                    CityName = apiResponse.Name,
                    Country = apiResponse.Sys.Country,
                    Temperature = apiResponse.Main.Temp,
                    FeelsLike = apiResponse.Main.FeelsLike,
                    TempMin = apiResponse.Main.TempMin,
                    TempMax = apiResponse.Main.TempMax,
                    Humidity = apiResponse.Main.Humidity,
                    Pressure = apiResponse.Main.Pressure,
                    WindSpeed = apiResponse.Wind.Speed,
                    Description = apiResponse.Weather[0].Description,
                    Icon = apiResponse.Weather[0].Icon,
                    DateTime = DateTimeOffset.FromUnixTimeSeconds(apiResponse.Dt).DateTime
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération de la météo: {ex.Message}");
            }
        }

        /// <summary>
        /// Récupère les prévisions météo sur 5 jours
        /// </summary>
        public async Task<ForecastData> GetForecastAsync(string cityName)
        {
            try
            {
                string url = $"{BASE_URL}/forecast?q={cityName}&appid={_apiKey}&units=metric&lang=fr";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Erreur lors de la récupération des prévisions: {response.StatusCode}");
                }

                string json = await response.Content.ReadAsStringAsync();
                OpenWeatherForecastResponse apiResponse = JsonConvert.DeserializeObject<OpenWeatherForecastResponse>(json);

                var forecastData = new ForecastData
                {
                    CityName = apiResponse.City.Name
                };

                var dailyForecasts = apiResponse.List
                    .GroupBy(f => DateTimeOffset.FromUnixTimeSeconds(f.Dt).Date)
                    .Select(g => g.OrderBy(f => Math.Abs(DateTimeOffset.FromUnixTimeSeconds(f.Dt).Hour - 12)).First())
                    .Take(5)
                    .Select(f => new DailyForecast
                    {
                        Date = DateTimeOffset.FromUnixTimeSeconds(f.Dt).DateTime,
                        Temp = f.Main.Temp,
                        TempMin = f.Main.TempMin,
                        TempMax = f.Main.TempMax,
                        Description = f.Weather[0].Description,
                        Icon = f.Weather[0].Icon,
                        Humidity = f.Main.Humidity,
                        WindSpeed = f.Wind.Speed
                    })
                    .ToList();

                forecastData.Forecasts = dailyForecasts;

                return forecastData;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des prévisions: {ex.Message}");
            }
        }

        public string GetWeatherIconUrl(string iconCode)
        {
            return $"https://openweathermap.org/img/wn/{iconCode}@2x.png";
        }
    }
}
