namespace EGET.App;

public partial class FavouritesPage : ContentPage
{
    public FavouritesPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (FavouriteStorage.IsCorollaFavourite())
        {
            ShowFavouriteVehicle();
        }
    }

    private void ShowFavouriteVehicle()
    {
        EmptyFavouritesView.IsVisible = false;
        FavouriteVehicleView.IsVisible = true;
    }
}