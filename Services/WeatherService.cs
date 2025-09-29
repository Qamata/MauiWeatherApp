using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MauiWeatherApp.Models;

namespace MauiWeatherApp.Services
{
    public class WeatherService
    {
        private const string ApiKey = "684dab621969facff2477f98aa00a5b6";
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

        private readonly HttpClient _httpClient;

        public WeatherService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<WeatherData> GetWeatherAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"{BaseUrl}?lat={latitude}&lon={longitude}&appid={ApiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonSerializer.Deserialize<WeatherData>(content);
                    return weatherData;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching weather: {ex.Message}");
                return null;
            }
        }

        public async Task<WeatherData> GetWeatherByCityAsync(string cityName)
        {
            try
            {
                var url = $"{BaseUrl}?q={cityName}&appid={ApiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonSerializer.Deserialize<WeatherData>(content);
                    return weatherData;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching weather: {ex.Message}");
                return null;
            }
        }
    }
}
