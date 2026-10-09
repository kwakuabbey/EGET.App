namespace EGET.App;

public partial class VehicleDetailsPage : ContentPage
{
    private Vehicle? selectedVehicle;

    // EXISTING COROLLA DETAILS PAGE
    public VehicleDetailsPage()
    {
        InitializeComponent();
    }

    // DETAILS PAGE FOR A SELECTED VEHICLE
    public VehicleDetailsPage(Vehicle vehicle)
    {
        InitializeComponent();

        selectedVehicle = vehicle;

        DisplayVehicle(vehicle);
    }

    // DISPLAY VEHICLE INFORMATION
    private void DisplayVehicle(Vehicle vehicle)
    {
        // VEHICLE IMAGE
        if (vehicle.PhotoPaths.Count > 0)
        {
            VehicleImage.Source =
                ImageSource.FromFile(vehicle.PhotoPaths[0]);
        }

        // VEHICLE NAME
        VehicleNameLabel.Text =
            $"{vehicle.Year} {FormatVehicleName(vehicle.Brand)} {FormatVehicleName(vehicle.Model)}";

        // VEHICLE PRICE
        VehiclePriceLabel.Text =
            $"GH₵ {FormatPrice(vehicle.Price)}";

        // VEHICLE INFORMATION
        VehicleYearLabel.Text =
            vehicle.Year;

        VehicleMileageLabel.Text =
            $"{vehicle.Mileage} km";

        VehicleFuelLabel.Text =
            vehicle.Fuel;

        VehicleTransmissionLabel.Text =
            vehicle.Transmission;

        VehicleLocationLabel.Text =
            $"{vehicle.City}, {vehicle.Region}";

        // VEHICLE DESCRIPTION
        VehicleDescriptionLabel.Text =
            vehicle.Description;
    }

    // FORMAT VEHICLE PRICE
    private string FormatPrice(string price)
    {
        if (decimal.TryParse(
            price.Replace(",", ""),
            out decimal amount))
        {
            return amount.ToString("N0");
        }

        return price;
    }

    // FORMAT VEHICLE NAME
    private string FormatVehicleName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return name;

        return string.Join(
            " ",
            name
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(word =>
                    char.ToUpper(word[0]) +
                    word.Substring(1).ToLower()));
    }

    // SAVE VEHICLE TO FAVOURITES
    private async void OnSaveFavouriteClicked(object sender, EventArgs e)
    {
        if (selectedVehicle != null)
        {
            FavouriteVehicleStorage.AddFavourite(selectedVehicle);

            await DisplayAlert(
                "Favourite",
                $"{FormatVehicleName(selectedVehicle.Brand)} {FormatVehicleName(selectedVehicle.Model)} has been saved to your favourites.",
                "OK");

            return;
        }

        // EXISTING COROLLA FAVOURITE
        FavouriteStorage.SaveCorolla();

        await Shell.Current.GoToAsync(nameof(FavouritesPage));
    }

    // CONTACT SELLER
    private async void OnContactSellerClicked(object sender, EventArgs e)
    {
        if (selectedVehicle == null)
        {
            await DisplayAlertAsync(
                "Contact Seller",
                "Messaging for this vehicle is not connected yet.",
                "OK");

            return;
        }

        var navigationParameters = new Dictionary<string, object>
    {
        { "SelectedVehicle", selectedVehicle }
    };

        await Shell.Current.GoToAsync(
            nameof(MessageSellerPage),
            navigationParameters);
    }
}
  