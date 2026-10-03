namespace EGET.App;

public partial class BrowsePage : ContentPage
{
    private const string VehicleName = "2020 Toyota Corolla";

    public BrowsePage()
    {
        InitializeComponent();
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(searchText))
        {
            VehicleCard.IsVisible = true;
            return;
        }

        VehicleCard.IsVisible =
            VehicleName.Contains(searchText, StringComparison.OrdinalIgnoreCase);
    }

    private async void OnViewDetailsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(VehicleDetailsPage));
    }
}