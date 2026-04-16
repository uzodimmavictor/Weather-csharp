using System;

namespace WeatherApp.Models
{
    public class FavoriteCity
    {
        public string CityName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime AddedDate { get; set; }

        public string FullName => string.IsNullOrEmpty(Country) ?
            CityName : $"{CityName}, {Country}";
    }
}
