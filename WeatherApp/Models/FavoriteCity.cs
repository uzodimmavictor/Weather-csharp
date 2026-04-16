using System;

namespace WeatherApp.Models
{
    public class FavoriteCity
    {
        public string CityName { get; set; }
        public string Country { get; set; }
        public DateTime AddedDate { get; set; }

        public string FullName => string.IsNullOrEmpty(Country) ?
            CityName : $"{CityName}, {Country}";
    }
}
