using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MauiWeatherApp.Models;
using MauiWeatherApp.Services;

namespace MauiWeatherApp.ViewModels
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        private readonly WeatherService _weatherService;
        private readonly LocationService _locationService;

        private WeatherData _weatherData;
        private string _errorMessage;
        private bool _isLoading;

        public WeatherData WeatherData
        {
            get => _weatherData;
            set
            {
                _weatherData = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }

        public WeatherViewModel()
        {
            _weatherService = new WeatherService();
            _locationService = new LocationService();

            RefreshCommand = new Command(async () => await GetWeatherAsync());
            SearchCommand = new Command<string>(async (city) => await SearchWeatherAsync(city));

            Task.Run(GetWeatherAsync);
        }

        private async Task GetWeatherAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var location = await _locationService.GetCurrentLocationAsync();

                if (location != null)
                {
                    var weather = await _weatherService.GetWeatherAsync(location.Latitude, location.Longitude);

                    if (weather != null)
                    {
                        WeatherData = weather;
                    }
                    else
                    {
                        ErrorMessage = "Failed to fetch weather data";
                    }
                }
                else
                {
                    ErrorMessage = "Unable to get location";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchWeatherAsync(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return;

            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var weather = await _weatherService.GetWeatherByCityAsync(cityName);

                if (weather != null)
                {
                    WeatherData = weather;
                }
                else
                {
                    ErrorMessage = "City not found";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
