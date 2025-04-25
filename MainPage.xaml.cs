using Microsoft.Maui.Controls;
using Microsoft.Maui.Maps;

namespace LocationTracking;

public partial class MainPage : ContentPage
{
    private bool _hasNavigated = false;

    public MainPage()
    {
        InitializeComponent();

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_hasNavigated)
        {
            _hasNavigated = true;
            await Shell.Current.GoToAsync(nameof(HeatMapPage));
        }
    }

    private async void OnHeatMapClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HeatMapPage));
    }
}



        //private void OnCounterClicked(object sender, EventArgs e)
        //{
        //    count++;

        //    if (count == 1)
        //        CounterBtn.Text = $"Clicked {count} time";
        //    else
        //        CounterBtn.Text = $"Clicked {count} times";

        //    SemanticScreenReader.Announce(CounterBtn.Text);
        //}
    

