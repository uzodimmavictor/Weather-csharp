using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class FavoritesService
    {
        private readonly string _favoritesFilePath;
        private List<FavoriteCity> _favorites;

        public FavoritesService()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appDataPath, "WeatherApp");

            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }

            _favoritesFilePath = Path.Combine(appFolder, "favorites.json");
            _favorites = new List<FavoriteCity>();

            LoadFavorites();
        }

        private void LoadFavorites()
        {
            try
            {
                if (File.Exists(_favoritesFilePath))
                {
                    string json = File.ReadAllText(_favoritesFilePath);
                    _favorites = JsonConvert.DeserializeObject<List<FavoriteCity>>(json) ?? new List<FavoriteCity>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des favoris: {ex.Message}");
                _favorites = new List<FavoriteCity>();
            }
        }

        private void SaveFavorites()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_favorites, Formatting.Indented);
                File.WriteAllText(_favoritesFilePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la sauvegarde des favoris: {ex.Message}");
            }
        }

        public List<FavoriteCity> GetFavorites()
        {
            return _favorites.OrderBy(f => f.CityName).ToList();
        }

        public bool AddFavorite(string cityName, string country)
        {
            if (IsFavorite(cityName))
            {
                return false;
            }

            _favorites.Add(new FavoriteCity
            {
                CityName = cityName,
                Country = country,
                AddedDate = DateTime.Now
            });

            SaveFavorites();
            return true;
        }

        public bool RemoveFavorite(string cityName)
        {
            var favorite = _favorites.FirstOrDefault(f =>
                f.CityName.Equals(cityName, StringComparison.OrdinalIgnoreCase));

            if (favorite != null)
            {
                _favorites.Remove(favorite);
                SaveFavorites();
                return true;
            }

            return false;
        }

        public bool IsFavorite(string cityName)
        {
            return _favorites.Any(f =>
                f.CityName.Equals(cityName, StringComparison.OrdinalIgnoreCase));
        }

        public void ClearFavorites()
        {
            _favorites.Clear();
            SaveFavorites();
        }
    }
}
