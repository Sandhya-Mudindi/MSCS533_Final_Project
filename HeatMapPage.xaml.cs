using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Linq;
using System.Threading.Tasks;
using LocationTracking.Services;
using LocationTracking.Models;
using System.Diagnostics;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace LocationTracking
{
    public partial class HeatMapPage : ContentPage
    {
        private readonly DatabaseService _dbService;

        public HeatMapPage(DatabaseService dbService)
        {
            InitializeComponent();
            _dbService = dbService;

            _dbService.LocationSaved += OnLocationSaved;

            _ = LoadHeatMap();
        }

        private async Task LoadHeatMap()
        {

            var locations = await _dbService.GetLocationsAsync();

            if (locations == null || locations.Count == 0)
            {
                Console.WriteLine("***************** No locations found to pin. *****************");
                return;
            }

            HeatMap.MapElements.Clear();

            foreach (var loc in locations)
            {
                AddCircle(loc);
            }

            var last = locations.LastOrDefault();
            if (last != null)
            {
                HeatMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Location(last.Latitude, last.Longitude),
                    Distance.FromKilometers(1)));
            }
        }

        // This is called every time a new location is saved
        private void OnLocationSaved(LocationData newLoc)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                AddCircle(newLoc);
            });
        }

        private void AddCircle(LocationData loc)
        {

            var circle = new Circle
            {
                Center = new Location(loc.Latitude, loc.Longitude),
                Radius = new Distance(5), // 5 meters
                StrokeColor = Colors.Transparent,
                FillColor = Colors.Blue.WithAlpha(0.9f),
                StrokeWidth = 0

            };

            HeatMap.MapElements.Add(circle);
        }
        //  Unsubscribe from the event when page disappears
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _dbService.LocationSaved -= OnLocationSaved;
        }
    }
}
