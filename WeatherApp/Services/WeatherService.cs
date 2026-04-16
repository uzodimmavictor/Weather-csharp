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
        private const string BASE_URL = "https://api.openweathermap.org/data/2.5";

        public WeatherService()
        {
            _httpClient = new HttpClient();
        }

        private string? GetApiKey()
        {
            string? apiKey = Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY");

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                string[] candidatePaths =
                {
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.local.json"),
                    Path.Combine(Directory.GetCurrentDirectory(), "appsettings.local.json"),
                    Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "appsettings.local.json"))
                };

                foreach (string configPath in candidatePaths.Distinct())
                {
                    if (!File.Exists(configPath))
                    {
                        continue;
                    }

                    string configJson = File.ReadAllText(configPath);
                    using JsonDocument doc = JsonDocument.Parse(configJson);

                    if (doc.RootElement.TryGetProperty("OpenWeatherApiKey", out JsonElement keyElement))
                    {
                        apiKey = keyElement.GetString();
                    }

                    if (!string.IsNullOrWhiteSpace(apiKey))
                    {
                        break;
                    }
                }
            }

            return apiKey;
        }

        private static void EnsureApiKeyConfigured(string? apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey) || string.Equals(apiKey, "votre_cle_api_ici", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Clé API manquante ou invalide. Définissez OPENWEATHER_API_KEY ou mettez une vraie clé dans appsettings.local.json (OpenWeatherApiKey)."
                );
            }
        }

        public async Task<WeatherData> GetCurrentWeatherAsync(string cityName)
        {
            try
            {
                string? apiKey = GetApiKey();
                EnsureApiKeyConfigured(apiKey);

                string encodedCity = Uri.EscapeDataString(cityName);
                string url = $"{BASE_URL}/weather?q={encodedCity}&appid={apiKey}&units=metric&lang=fr";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Erreur lors de la récupération de la météo: {response.StatusCode}");
                }

                string json = await response.Content.ReadAsStringAsync();
                OpenWeatherResponse apiResponse = JsonConvert.DeserializeObject<OpenWeatherResponse>(json)
                    ?? throw new Exception("Réponse API invalide pour la météo actuelle.");

                if (apiResponse.Weather == null || apiResponse.Weather.Count == 0)
                {
                    throw new Exception("Réponse API incomplète: informations météo absentes.");
                }

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

        public async Task<ForecastData> GetForecastAsync(string cityName)
        {
            try
            {
                string? apiKey = GetApiKey();
                EnsureApiKeyConfigured(apiKey);

                string encodedCity = Uri.EscapeDataString(cityName);
                string url = $"{BASE_URL}/forecast?q={encodedCity}&appid={apiKey}&units=metric&lang=fr";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Erreur lors de la récupération des prévisions: {response.StatusCode}");
                }

                string json = await response.Content.ReadAsStringAsync();
                OpenWeatherForecastResponse apiResponse = JsonConvert.DeserializeObject<OpenWeatherForecastResponse>(json)
                    ?? throw new Exception("Réponse API invalide pour les prévisions.");

                if (apiResponse.List == null || apiResponse.List.Count == 0)
                {
                    throw new Exception("Réponse API incomplète: liste de prévisions absente.");
                }

                if (apiResponse.City == null)
                {
                    throw new Exception("Réponse API incomplète: informations de ville absentes.");
                }

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
