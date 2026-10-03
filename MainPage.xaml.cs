namespace EGET.App;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnBuyCarTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(BrowsePage));
    }

    private async void OnFavouritesTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FavouritesPage));
    }
}