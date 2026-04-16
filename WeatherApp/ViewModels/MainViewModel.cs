using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WeatherApp.Helpers;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    /// <summary>
    /// ViewModel principal pour la fenêtre principale
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly WeatherService _weatherService;
        private readonly FavoritesService _favoritesService;

        // Propriétés pour le binding
        private string _searchCity;
        private WeatherData _currentWeather;
        private ForecastData _forecastData;
        private bool _isLoading;
        private string _errorMessage;
        private string _statusMessage;
        private bool _isFavorite;

        public MainViewModel()
        {
            _weatherService = new WeatherService();
            _favoritesService = new FavoritesService();

            FavoriteCities = new ObservableCollection<FavoriteCity>();

            // Initialiser les commandes
            SearchCommand = new RelayCommand(async _ => await SearchWeatherAsync(), _ => !string.IsNullOrWhiteSpace(SearchCity) && !IsLoading);
            AddToFavoritesCommand = new RelayCommand(_ => AddToFavorites(), _ => CurrentWeather != null && !IsFavorite);
            RemoveFromFavoritesCommand = new RelayCommand(_ => RemoveFromFavorites(), _ => CurrentWeather != null && IsFavorite);
            LoadFavoriteCityCommand = new RelayCommand(async city => await LoadFavoriteCityWeatherAsync(city as FavoriteCity));
            DeleteFavoriteCommand = new RelayCommand(city => DeleteFavorite(city as FavoriteCity));

            // Charger les favoris au démarrage
            LoadFavorites();
        }

        #region Propriétés

        public string SearchCity
        {
            get => _searchCity;
            set => SetProperty(ref _searchCity, value);
        }

        public WeatherData CurrentWeather
        {
            get => _currentWeather;
            set
            {
                SetProperty(ref _currentWeather, value);
                UpdateFavoriteStatus();
            }
        }

        public ForecastData ForecastData
        {
            get => _forecastData;
            set => SetProperty(ref _forecastData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public bool IsFavorite
        {
            get => _isFavorite;
            set => SetProperty(ref _isFavorite, value);
        }

        public ObservableCollection<FavoriteCity> FavoriteCities { get; }

        #endregion

        #region Commandes

        public ICommand SearchCommand { get; }
        public ICommand AddToFavoritesCommand { get; }
        public ICommand RemoveFromFavoritesCommand { get; }
        public ICommand LoadFavoriteCityCommand { get; }
        public ICommand DeleteFavoriteCommand { get; }

        #endregion

        #region Méthodes

        private async System.Threading.Tasks.Task SearchWeatherAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchCity))
                return;

            IsLoading = true;
            ErrorMessage = string.Empty;
            StatusMessage = $"Recherche de la météo pour {SearchCity}...";

            try
            {
                // Récupérer la météo actuelle
                CurrentWeather = await _weatherService.GetCurrentWeatherAsync(SearchCity);

                // Récupérer les prévisions
                ForecastData = await _weatherService.GetForecastAsync(SearchCity);

                StatusMessage = $"Météo chargée pour {CurrentWeather.CityName}";
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur: {ex.Message}";
                StatusMessage = string.Empty;
                CurrentWeather = null;
                ForecastData = null;
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Ajouter la ville actuelle aux favoris
        /// </summary>
        private void AddToFavorites()
        {
            if (CurrentWeather == null) return;

            bool added = _favoritesService.AddFavorite(CurrentWeather.CityName, CurrentWeather.Country);

            if (added)
            {
                LoadFavorites();
                StatusMessage = $"{CurrentWeather.CityName} ajoutée aux favoris";
                UpdateFavoriteStatus();
            }
            else
            {
                StatusMessage = $"{CurrentWeather.CityName} est déjà dans les favoris";
            }
        }

        /// <summary>
        /// Retirer la ville actuelle des favoris
        /// </summary>
        private void RemoveFromFavorites()
        {
            if (CurrentWeather == null) return;

            bool removed = _favoritesService.RemoveFavorite(CurrentWeather.CityName);

            if (removed)
            {
                LoadFavorites();
                StatusMessage = $"{CurrentWeather.CityName} retirée des favoris";
                UpdateFavoriteStatus();
            }
        }

        /// <summary>
        /// Charger la météo pour une ville favorite
        /// </summary>
        private async System.Threading.Tasks.Task LoadFavoriteCityWeatherAsync(FavoriteCity city)
        {
            if (city == null) return;

            SearchCity = city.CityName;
            await SearchWeatherAsync();
        }

        /// <summary>
        /// Supprimer une ville des favoris
        /// </summary>
        private void DeleteFavorite(FavoriteCity city)
        {
            if (city == null) return;

            var result = MessageBox.Show(
                $"Voulez-vous vraiment supprimer {city.FullName} des favoris ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _favoritesService.RemoveFavorite(city.CityName);
                LoadFavorites();
                UpdateFavoriteStatus();
                StatusMessage = $"{city.CityName} supprimée des favoris";
            }
        }

        /// <summary>
        /// Charger la liste des villes favorites
        /// </summary>
        private void LoadFavorites()
        {
            FavoriteCities.Clear();
            var favorites = _favoritesService.GetFavorites();

            foreach (var favorite in favorites)
            {
                FavoriteCities.Add(favorite);
            }
        }

        /// <summary>
        /// Mettre à jour le statut de favori pour la ville actuelle
        /// </summary>
        private void UpdateFavoriteStatus()
        {
            if (CurrentWeather != null)
            {
                IsFavorite = _favoritesService.IsFavorite(CurrentWeather.CityName);
            }
            else
            {
                IsFavorite = false;
            }
        }

        /// <summary>
        /// Initialiser l'application en chargeant le premier favori
        /// </summary>
        public async System.Threading.Tasks.Task InitializeAsync()
        {
            if (FavoriteCities.Count > 0)
            {
                var firstFavorite = FavoriteCities[0];
                await LoadFavoriteCityWeatherAsync(firstFavorite);
            }
        }

        #endregion
    }
}
