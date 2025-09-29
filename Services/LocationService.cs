using Microsoft.Maui.Devices.Sensors;
using MauiWeatherApp.Models;

public class LocationService
{
    public async Task<MauiWeatherApp.Models.Location> GetCurrentLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                return new MauiWeatherApp.Models.Location
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                };
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting location: {ex.Message}");
            return null;
        }
    }
}