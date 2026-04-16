using System;
using System.Collections.Generic;

namespace WeatherApp.Models
{
    public class ForecastData
    {
        public string CityName { get; set; }
        public List<DailyForecast> Forecasts { get; set; } = new List<DailyForecast>();
    }

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

        public string DayName => Date.ToString("dddd");
        public string DateDisplay => Date.ToString("dd/MM");
        public string TempDisplay => $"{Math.Round(Temp)}°C";
        public string TempRangeDisplay => $"{Math.Round(TempMin)}° - {Math.Round(TempMax)}°";
    }
}
