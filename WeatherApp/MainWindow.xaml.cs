using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using WeatherApp.ViewModels;

namespace WeatherApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        if (DataContext is MainViewModel viewModel)
        {
            _ = viewModel.InitializeAsync();
            viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is MainViewModel viewModel && e.PropertyName == nameof(MainViewModel.IsDarkMode))
        {
            ApplyTheme(viewModel.IsDarkMode);
        }
    }

    private void ApplyTheme(bool isDarkMode)
    {
        if (isDarkMode)
        {
            SetBrush("WindowBackgroundBrush", "#0F172A");
            SetBrush("CardBackgroundBrush", "#111827");
            SetBrush("PrimaryTextBrush", "#E5E7EB");
            SetBrush("SecondaryTextBrush", "#9CA3AF");
            SetBrush("InputBackgroundBrush", "#1F2937");
            SetBrush("InputBorderBrush", "#374151");
            SetBrush("SectionBorderBrush", "#334155");
            SetBrush("StatusBackgroundBrush", "#1E293B");
            SetBrush("StatusTextBrush", "#CBD5E1");
            SetBrush("ErrorBackgroundBrush", "#7F1D1D");
            SetBrush("ErrorTextBrush", "#FCA5A5");
            SetBrush("HoverBackgroundBrush", "#1F2937");
        }
        else
        {
            SetBrush("WindowBackgroundBrush", "#F0F4F8");
            SetBrush("CardBackgroundBrush", "#FFFFFF");
            SetBrush("PrimaryTextBrush", "#1F2937");
            SetBrush("SecondaryTextBrush", "#6B7280");
            SetBrush("InputBackgroundBrush", "#FFFFFF");
            SetBrush("InputBorderBrush", "#D1D5DB");
            SetBrush("SectionBorderBrush", "#E5E7EB");
            SetBrush("StatusBackgroundBrush", "#E5E7EB");
            SetBrush("StatusTextBrush", "#374151");
            SetBrush("ErrorBackgroundBrush", "#FEE2E2");
            SetBrush("ErrorTextBrush", "#DC2626");
            SetBrush("HoverBackgroundBrush", "#F3F4F6");
        }
    }

    private void SetBrush(string resourceKey, string colorHex)
    {
        var color = (Color)ColorConverter.ConvertFromString(colorHex);
        Resources[resourceKey] = new SolidColorBrush(color);
    }
}