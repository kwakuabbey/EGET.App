namespace EGET.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(BrowsePage), typeof(BrowsePage));
        Routing.RegisterRoute(nameof(VehicleDetailsPage), typeof(VehicleDetailsPage));
        Routing.RegisterRoute(nameof(FavouritesPage), typeof(FavouritesPage));
        Routing.RegisterRoute(nameof(SellVehiclePage), typeof(SellVehiclePage));
    }
}