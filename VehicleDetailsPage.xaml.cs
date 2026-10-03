namespace EGET.App;

public partial class VehicleDetailsPage : ContentPage
{
    public VehicleDetailsPage()
    {
        InitializeComponent();
    }

    private async void OnSaveFavouriteClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FavouritesPage));
    }
}