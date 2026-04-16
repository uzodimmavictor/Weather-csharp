using System;

namespace WeatherApp.Models
{
    /// <summary>
    /// Représente les données météo actuelles pour une ville
    /// </summary>
    public class WeatherData
    {
        public string CityName { get; set; }
        public string Country { get; set; }
        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public DateTime DateTime { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public int Pressure { get; set; }

        // Propriété calculée pour afficher la température en Celsius avec le symbole
        public string TemperatureDisplay => $"{Math.Round(Temperature)}°C";

        // Propriété calculée pour la description capitalisée
        public string DescriptionCapitalized =>
            string.IsNullOrEmpty(Description) ? "" :
            char.ToUpper(Description[0]) + Description.Substring(1);
    }
}
