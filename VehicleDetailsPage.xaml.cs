namespace EGET.App;

public partial class VehicleDetailsPage : ContentPage
{
    public VehicleDetailsPage()
    {
        InitializeComponent();
    }

    private async void OnSaveFavouriteClicked(object sender, EventArgs e)
    {
        FavouriteStorage.SaveCorolla();

        await Shell.Current.GoToAsync(nameof(FavouritesPage));
    }
}