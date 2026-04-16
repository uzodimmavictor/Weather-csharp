using System;
using System.Collections.Generic;

namespace WeatherApp.Models
{
    /// <summary>
    /// Représente les prévisions météo sur plusieurs jours
    /// </summary>
    public class ForecastData
    {
        public string CityName { get; set; }
        public List<DailyForecast> Forecasts { get; set; } = new List<DailyForecast>();
    }

    /// <summary>
    /// Représente la prévision météo pour un jour spécifique
    /// </summary>
    public class DailyForecast
    {
        public DateTime Date { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public double Temp { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }

        // Propriétés calculées pour l'affichage
        public string DayName => Date.ToString("dddd");
        public string DateDisplay => Date.ToString("dd/MM");
        public string TempDisplay => $"{Math.Round(Temp)}°C";
        public string TempRangeDisplay => $"{Math.Round(TempMin)}° - {Math.Round(TempMax)}°";
    }
}
