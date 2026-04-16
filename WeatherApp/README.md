# 🌤️ Application Météo Provence

Une application de météo Windows native développée en C# avec WPF pour retrouver notre beau soleil de Provence !

## 📋 Fonctionnalités

✅ **Recherche de météo par ville** - Obtenez les informations météo actuelles pour n'importe quelle ville
✅ **Prévisions sur 5 jours** - Consultez les prévisions détaillées
✅ **Gestion des favoris** - Sauvegardez vos villes préférées pour un accès rapide
✅ **Interface moderne** - Design épuré et intuitif
✅ **Données en temps réel** - Informations provenant d'OpenWeatherMap

## 🚀 Installation et Configuration

### Prérequis

- Windows 10/11
- .NET 8.0 SDK ou supérieur
- Visual Studio 2022 ou VS Code (optionnel)

### Étape 1: Obtenir une clé API OpenWeatherMap

1. Créez un compte gratuit sur [OpenWeatherMap](https://openweathermap.org/)
2. Connectez-vous et accédez à [API Keys](https://home.openweathermap.org/api_keys)
3. Générez une nouvelle clé API (gratuite jusqu'à 1000 appels/jour)
4. Copiez votre clé API

### Étape 2: Configurer l'application

1. Copiez `appsettings.local.example.json` en `appsettings.local.json`
2. Remplacez `votre_cle_api_ici` par votre clé API:
   ```json
   {
     "OpenWeatherApiKey": "votre_cle_api_ici"
   }
   ```
3. Alternative: définissez la variable d'environnement `OPENWEATHER_API_KEY`

`appsettings.local.json` est ignoré par Git pour éviter de versionner votre clé API.

### Étape 3: Compiler et exécuter

#### Option 1: Ligne de commande
```bash
cd WeatherApp
dotnet build
dotnet run
```

#### Option 2: Visual Studio
1. Ouvrez le fichier `WeatherApp.csproj`
2. Appuyez sur F5 pour compiler et exécuter

## 🎯 Utilisation

### Rechercher une ville
1. Entrez le nom d'une ville dans la barre de recherche
2. Cliquez sur "🔍 Rechercher" ou appuyez sur Entrée
3. La météo actuelle et les prévisions s'affichent

### Gérer les favoris
- **Ajouter aux favoris**: Cliquez sur "⭐ Favori" après avoir recherché une ville
- **Charger un favori**: Cliquez sur une ville dans la liste des favoris (barre latérale)
- **Supprimer un favori**: Cliquez sur l'icône 🗑️ à côté de la ville

### Informations affichées
- 🌡️ Température actuelle et ressentie
- 📊 Humidité, pression atmosphérique
- 💨 Vitesse du vent
- 📅 Prévisions sur 5 jours avec températures min/max

## 📁 Structure du Projet

```
WeatherApp/
├── Models/                  # Modèles de données
│   ├── WeatherData.cs      # Données météo actuelles
│   ├── ForecastData.cs     # Prévisions
│   ├── FavoriteCity.cs     # Villes favorites
│   └── OpenWeatherResponse.cs # Modèles API
├── Services/               # Services métier
│   ├── WeatherService.cs   # Récupération données météo
│   └── FavoritesService.cs # Gestion des favoris
├── ViewModels/             # ViewModels MVVM
│   └── MainViewModel.cs    # ViewModel principal
├── Helpers/                # Utilitaires
│   ├── RelayCommand.cs     # Gestion des commandes
│   ├── ViewModelBase.cs    # Classe de base ViewModel
│   └── Converters.cs       # Converteurs XAML
├── MainWindow.xaml         # Interface utilisateur
└── MainWindow.xaml.cs      # Code-behind
```

## 🏗️ Architecture

L'application utilise le pattern **MVVM (Model-View-ViewModel)** :

- **Models**: Classes représentant les données (météo, prévisions, favoris)
- **Views**: Interface utilisateur en XAML
- **ViewModels**: Logique de présentation et liaison de données
- **Services**: Logique métier (appels API, sauvegarde)

## 🔧 Technologies Utilisées

- **WPF** - Framework d'interface graphique Windows
- **C# 12** - Langage de programmation
- **.NET 8.0** - Framework d'exécution
- **Newtonsoft.Json** - Gestion du JSON
- **OpenWeatherMap API** - Données météorologiques

## 📦 Packages NuGet

- `Newtonsoft.Json` - Désérialisation JSON
- `System.Net.Http` - Appels HTTP vers l'API

## 🌟 Améliorations Futures (Bonus)

- [ ] Carte interactive avec localisation
- [ ] Thème sombre/clair
- [ ] Graphiques de tendances météo
- [ ] Notifications pour alertes météo
- [ ] Support multilingue
- [ ] Géolocalisation automatique
- [ ] Widget pour le bureau Windows

## 🐛 Résolution de Problèmes

### Erreur: "Erreur lors de la récupération de la météo: 401"
➡️ Votre clé API n'est pas valide ou n'est pas encore activée (attendre 10-15 minutes après création)

### Erreur: "Erreur lors de la récupération de la météo: 404"
➡️ Le nom de la ville est incorrect ou n'existe pas dans la base de données

### Les favoris ne se sauvegardent pas
➡️ Vérifiez les permissions d'écriture dans `%AppData%\WeatherApp\`

### L'application ne se lance pas
➡️ Vérifiez que .NET 8.0 SDK est installé : `dotnet --version`

## 📄 Licence

Ce projet est un projet éducatif développé pour apprendre le développement d'applications Windows avec C# et WPF.

## 👨‍💻 Auteur

Développé pour retrouver notre beau soleil de Provence ! ☀️

---

**Note**: L'API OpenWeatherMap gratuite limite à 1000 appels par jour. Pour un usage commercial, consultez leurs plans payants.
