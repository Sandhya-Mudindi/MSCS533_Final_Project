using Microsoft.Maui.Devices.Sensors;
using System;
using System.Threading.Tasks;

namespace LocationTracking.Services
{
    public class LocationService
    {
        public async Task<Location> GetCurrentLocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync()
                                ?? await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best));

                return location;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Location error: {ex.Message}");
                return null!;
            }
        }
    }
}
