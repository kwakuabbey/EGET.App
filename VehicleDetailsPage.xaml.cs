namespace EGET.App;

public partial class VehicleDetailsPage : ContentPage
{
    private Vehicle? selectedVehicle;

    // STORE THE VEHICLE PHOTOS
    private List<string> vehiclePhotoPaths = new();

    // TRACK THE CURRENT PHOTO
    private int currentPhotoIndex = 0;

    // EXISTING COROLLA DETAILS PAGE
    public VehicleDetailsPage()
    {
        InitializeComponent();

        // DISPLAY THE DEFAULT COROLLA IMAGE
        vehiclePhotoPaths = new List<string>
        {
            "corolla1.png"
        };

        currentPhotoIndex = 0;

        UpdatePhotoGallery();
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
        // LOAD VEHICLE PHOTOS
        vehiclePhotoPaths = vehicle.PhotoPaths != null
            ? new List<string>(vehicle.PhotoPaths)
            : new List<string>();

        currentPhotoIndex = 0;

        // DISPLAY THE PHOTO GALLERY
        UpdatePhotoGallery();

        // VEHICLE NAME
        VehicleNameLabel.Text =
            $"{vehicle.Year} {FormatVehicleName(vehicle.Brand)} {FormatVehicleName(vehicle.Model)}";

        // VEHICLE PRICE
        VehiclePriceLabel.Text =
            $"GH₵ {FormatPrice(vehicle.Price)}";

        // VEHICLE INFORMATION
        VehicleYearLabel.Text = vehicle.Year;

        VehicleMileageLabel.Text =
            $"{vehicle.Mileage} km";

        VehicleFuelLabel.Text = vehicle.Fuel;

        VehicleTransmissionLabel.Text =
            vehicle.Transmission;

        VehicleLocationLabel.Text =
            $"{vehicle.City}, {vehicle.Region}";

        // VEHICLE DESCRIPTION
        VehicleDescriptionLabel.Text =
            vehicle.Description;
    }

    // UPDATE THE DISPLAYED PHOTO AND CONTROLS
    private void UpdatePhotoGallery()
    {
        // HANDLE A VEHICLE WITH NO PHOTOS
        if (vehiclePhotoPaths.Count == 0)
        {
            VehicleImage.Source = "corolla1.png";

            PhotoCounterLabel.Text = "No photos";

            PreviousPhotoButton.IsEnabled = false;
            NextPhotoButton.IsEnabled = false;

            return;
        }

        // KEEP THE CURRENT INDEX WITHIN RANGE
        if (currentPhotoIndex < 0)
        {
            currentPhotoIndex = 0;
        }

        if (currentPhotoIndex >= vehiclePhotoPaths.Count)
        {
            currentPhotoIndex = vehiclePhotoPaths.Count - 1;
        }

        // DISPLAY THE CURRENT PHOTO
        VehicleImage.Source =
            ImageSource.FromFile(
                vehiclePhotoPaths[currentPhotoIndex]);

        // UPDATE PHOTO COUNTER
        PhotoCounterLabel.Text =
            $"{currentPhotoIndex + 1} of {vehiclePhotoPaths.Count}";

        // ENABLE OR DISABLE NAVIGATION BUTTONS
        PreviousPhotoButton.IsEnabled =
            currentPhotoIndex > 0;

        NextPhotoButton.IsEnabled =
            currentPhotoIndex < vehiclePhotoPaths.Count - 1;
    }

    // SHOW THE PREVIOUS PHOTO
    private void OnPreviousPhotoClicked(
        object sender,
        EventArgs e)
    {
        if (currentPhotoIndex > 0)
        {
            currentPhotoIndex--;

            UpdatePhotoGallery();
        }
    }

    // SHOW THE NEXT PHOTO
    private void OnNextPhotoClicked(
        object sender,
        EventArgs e)
    {
        if (currentPhotoIndex < vehiclePhotoPaths.Count - 1)
        {
            currentPhotoIndex++;

            UpdatePhotoGallery();
        }
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
    private async void OnSaveFavouriteClicked(
        object sender,
        EventArgs e)
    {
        if (selectedVehicle != null)
        {
            FavouriteVehicleStorage.AddFavourite(
                selectedVehicle);

            await DisplayAlertAsync(
                "Favourite",
                $"{FormatVehicleName(selectedVehicle.Brand)} {FormatVehicleName(selectedVehicle.Model)} has been saved to your favourites.",
                "OK");

            return;
        }

        // EXISTING COROLLA FAVOURITE
        FavouriteStorage.SaveCorolla();

        await Shell.Current.GoToAsync(
            nameof(FavouritesPage));
    }

    // CONTACT SELLER
    private async void OnContactSellerClicked(
        object sender,
        EventArgs e)
    {
        if (selectedVehicle == null)
        {
            await DisplayAlertAsync(
                "Contact Seller",
                "Messaging for this vehicle is not connected yet.",
                "OK");

            return;
        }

        var navigationParameters =
            new Dictionary<string, object>
            {
                { "SelectedVehicle", selectedVehicle }
            };

        await Shell.Current.GoToAsync(
            nameof(MessageSellerPage),
            navigationParameters);
    }
}