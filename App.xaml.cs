using LocationTracking.Services;
using System.Timers;
using MauiLocation = Microsoft.Maui.Devices.Sensors.Location;



namespace LocationTracking
{
    public partial class App : Application
    {
        private readonly DatabaseService _dbService;
        private readonly LocationService _locationService;
        private readonly System.Timers.Timer _timer;
        private MauiLocation _lastLocation;
        private DateTime _lastSavedTime = DateTime.MinValue;

        public App(DatabaseService dbService)
        {
            InitializeComponent();
            Console.WriteLine("********************  Appp intialize ********************************");
            MainPage = new AppShell(); // This is Shell-based main page

            _dbService = dbService;
            _locationService = new LocationService();

            // Timer to save location every 1 seconds
            Console.WriteLine("****************** waiting 30 seconds *******************");
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += async (s, e) => await TrackLocationAsync();
            _timer.Start();


        }

        private async Task OnLocationUpdated(MauiLocation newLocation)


        {
            if (newLocation == null)
                return;

            if (_lastLocation == null ||
                (DateTime.Now - _lastSavedTime > TimeSpan.FromSeconds(1) &&
                 DistanceBetween(_lastLocation, newLocation) > 1))
            {
                // Save to DB (async context assumed)
                var locationData = new LocationTracking.Models.LocationData
                {
                    Latitude = newLocation.Latitude,
                    Longitude = newLocation.Longitude,
                    Timestamp = DateTime.Now
                };
                await _dbService.SaveLocationAsync(locationData);
                Console.WriteLine($"Saved location: {newLocation.Latitude}, {newLocation.Longitude}");

                _lastLocation = newLocation;
                _lastSavedTime = DateTime.Now;
            }
        }

        double DistanceBetween(MauiLocation loc1, MauiLocation loc2)

        {
            var R = 6371e3; // Earth radius in meters
            var lat1 = loc1.Latitude * Math.PI / 180;
            var lat2 = loc2.Latitude * Math.PI / 180;
            var dLat = (loc2.Latitude - loc1.Latitude) * Math.PI / 180;
            var dLon = (loc2.Longitude - loc1.Longitude) * Math.PI / 180;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = R * c;
            return distance; 
        }

        private async Task TrackLocationAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var currentLocation = await _locationService.GetCurrentLocationAsync();
                if (currentLocation != null)
                {
                    
                    await OnLocationUpdated(currentLocation);
                }
                else
                {
                    Console.WriteLine("*****************   No location found.**********************************");
                }
            });
        }
    

       


    }
}