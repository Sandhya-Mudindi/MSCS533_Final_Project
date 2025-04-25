using Microsoft.Maui.Controls;
using LocationTracking;


namespace LocationTracking

{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HeatMapPage), typeof(HeatMapPage));

        }
    }
}
