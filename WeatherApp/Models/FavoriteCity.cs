using System;

namespace WeatherApp.Models
{
    /// <summary>
    /// Représente une ville favorite sauvegardée par l'utilisateur
    /// </summary>
    public class FavoriteCity
    {
        public string CityName { get; set; }
        public string Country { get; set; }
        public DateTime AddedDate { get; set; }

        // Affichage complet de la ville
        public string FullName => string.IsNullOrEmpty(Country) ?
            CityName : $"{CityName}, {Country}";
    }
}
